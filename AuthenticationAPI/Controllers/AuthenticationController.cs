using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AuthenticationAPI.DataAccessLayer;
using AuthenticationAPI.Models;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly MSSQLContext _context;
        private IConfiguration _config;

        public AuthenticationController(MSSQLContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<User>> Me()
        {
            var UserId = User
                .Claims
                .FirstOrDefault();
            if (UserId == null)
            {
                return NotFound();
            }

            User user = await _context.User.AsQueryable().Where(x => x.Id == Convert.ToInt32(UserId.Value)).FirstAsync();

            if (user.Id > 0 )
            {
                return Ok(new {
                    Id = user.Id,
                    Email = user.Email
                });
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(LoginView data)
        {
            string salt = Encryption.GetSalt();
            string hash = Encryption.CreateSHAHash(data.Password, salt);
            User user = new User();
            user.Email = data.Email;
            bool isUserExists = UserExists(data.Email);
            if (isUserExists)
            {
                return BadRequest(new { message = "Email already taken!" });
            }
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            JWT jwt = new JWT(_config);
            string token = jwt.CreateJWTToken(user);

            return Ok(new {
                Email = user.Email,
                Token = token
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Login(LoginView data)
        {
            User user = await _context.User.FirstAsync(x => x.Email == data.Email);
            string hash = Encryption.CreateSHAHash(data.Password, user.PasswordSalt);
            if (user.PasswordHash == hash)
            {
                JWT jwt = new JWT(_config);
                string token = jwt.CreateJWTToken(user);

                return Ok(new {
                    Email = user.Email,
                    Token = token
                });
            }

            return BadRequest(new {
                Message = "Email or password is wrong!"
            });
        }

        private bool UserExists(string email)
        {
            return _context.User.AsQueryable().Any(e => e.Email == email);
        }
    }
}

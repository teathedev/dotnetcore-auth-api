using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }
    public class LoginView
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationAPI.Models;

namespace AuthenticationAPI.DataAccessLayer
{
    public class MSSQLContext : DbContext
    {
        public MSSQLContext()
        {

        }

        public MSSQLContext(DbContextOptions<MSSQLContext> opt) : base(opt)
        {

        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("sq_user", schema: "shared")
                .StartsAt(1)
                .IncrementsBy(1);
            
            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEXT VALUE FOR shared.sq_user");

            modelBuilder.Entity<User>()
                .HasAlternateKey(p => p.Email);
        }
    }
}

using Common.Model;
using Common.Model.Auth;
using Identity.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Domain
{
    public class IdentityDbContext : IdentityDbContext<User>, IDatabaseContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
           : base(options)
        {
        }

        public AuthenticatedUser CurrentUser { get; set; }

        public string Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}


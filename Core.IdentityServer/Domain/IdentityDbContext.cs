using Common.Model;
using IdentityServer.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Domain
{
    public class IdentityDbContext : DbContext, IDatabaseContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public AuthenticatedUser CurrentUser { get; set; }
        public string Token { get; set; }
    }
}


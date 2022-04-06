using Common.Model;
using Common.Model.Auth;
using Identity.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Prato.IdentityProvider.Entities;

namespace Identity.Domain
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                        .HasOne(u => u.User)
                        .WithMany(ur => ur.UserRoles)
                        .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserRole>()
                        .HasOne(r => r.Role)
                        .WithMany(ur => ur.UserRoles)
                        .HasForeignKey(r => r.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}


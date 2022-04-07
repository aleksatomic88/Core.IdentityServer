using Core.Users.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.Users.DAL
{
    public class CoreUsersDbContext : DbContext
    {
        public CoreUsersDbContext(DbContextOptions options)
             : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
}

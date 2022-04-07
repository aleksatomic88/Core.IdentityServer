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
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
}

using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Users.DAL
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions options)
             : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.PhoneNumber)
                        .IsUnique();

            //modelBuilder.Entity<User>()
            //            .ToTable("Users", b => b.IsTemporal());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.LogTo(Console.WriteLine);
    }
}

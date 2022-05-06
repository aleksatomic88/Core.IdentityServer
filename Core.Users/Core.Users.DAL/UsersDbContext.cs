using Common.Interface;
using Common.Model;
using Core.Users.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Core.Users.DAL
{
    public class UsersDbContext : DbContext, IDatabaseContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
             : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public AuthenticatedUser CurrentUser { get; set; } = new AuthenticatedUser { Id = 1 };
        public string Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.PhoneNumber)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .ToTable("Users", b => b.IsTemporal());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(message => Debug.WriteLine(message));
            // optionsBuilder.LogTo(Console.WriteLine);

    }
}

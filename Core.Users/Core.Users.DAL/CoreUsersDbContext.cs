using Core.Users.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Users.DAL
{
    public class CoreUsersDbContext : DbContext
    {
        public CoreUsersDbContext(DbContextOptions options)
             : base(options)
        {

        }

        public DbSet<WeatherForecasts> WeatherForecasts { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
}

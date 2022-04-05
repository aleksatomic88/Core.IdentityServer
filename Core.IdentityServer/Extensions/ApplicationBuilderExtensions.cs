using Identity.Domain;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Identity.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder webHost)
        {
            var serviceScopeFactory = (IServiceScopeFactory)webHost.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                var identityDbContext = services.GetRequiredService<IdentityDbContext>();
                var persistantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
                var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();

                var env = services.GetRequiredService<IWebHostEnvironment>();

                var dbInitializer = services.GetRequiredService<IdentityDatabaseInitializer>();
                var dbSeed = services.GetRequiredService<IdentityDatabaseSeed>();

                Log.Information("Environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                identityDbContext.Database.EnsureDeleted();
                persistantDbContext.Database.EnsureDeleted();
                configurationDbContext.Database.EnsureDeleted();

                identityDbContext.Database.Migrate();
                persistantDbContext.Database.Migrate();
                configurationDbContext.Database.Migrate();
                Log.Information("Database migrations executed.");

                dbInitializer.Initialize();
                Log.Information("Database initialized with required data.");

                dbSeed.Seed();
                Log.Information("Database seeded with test data.");
            }

            return webHost;
        }
    }
}


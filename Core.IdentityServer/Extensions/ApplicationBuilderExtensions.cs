using IdentityServer.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace IdentityServer.Extensions
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
                // var env = services.GetRequiredService<IWebHostEnvironment>();

                var dbInitializer = services.GetRequiredService<IdentityDatabaseInitializer>();
                var dbSeed = services.GetRequiredService<IdentityDatabaseSeed>();

                Log.Information("Environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                identityDbContext.Database.EnsureDeleted();
                identityDbContext.Database.EnsureCreated(); ;
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


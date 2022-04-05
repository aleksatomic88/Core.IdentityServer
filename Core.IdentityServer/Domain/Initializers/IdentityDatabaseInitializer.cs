using Identity.Domain.Initializers;
using Identity.Domain.Model;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Identity.Domain
{
    /// <summary>
    /// Populates db with data that must exist for app to work properly
    /// </summary>
    public class IdentityDatabaseInitializer
    {
        private readonly IdentityDbContext ctx;
        private readonly UserManager<User> userManager;
        private readonly RolesInitializer rolesInitializer;
        private readonly ConfigurationDbContext configurationDbContext;

        public IdentityDatabaseInitializer(
            IdentityDbContext ctx,
            UserManager<User> userManager,
            ConfigurationDbContext configurationDbContext,
            RolesInitializer rolesInitializer
            )
        {
            this.ctx = ctx;
            this.userManager = userManager;
            this.rolesInitializer = rolesInitializer;
            this.configurationDbContext = configurationDbContext;
        }

        public void Initialize()
        {
            InsertRoles();
            InsertUsers();
            InsertClientsAndResources();
        }

        private void InsertRoles()
        {
            rolesInitializer.Initialize();
        }

        private void InsertUsers()
        {
            var superAdmin = ctx.Users.Where(e => e.Email == "superadmin@super.admin").IgnoreQueryFilters().FirstOrDefault();

            if (superAdmin == null)
            {
                var identityResult = userManager.CreateAsync(new User
                {
                    UserName = "superadmin@super.admin",
                    Email = "superadmin@super.admin",
                    Name = "Super Admin",
                    EmailConfirmed = true,
                    Deleted = false
                }, "Pass123!").Result;

                ctx.SaveChanges();
            }

            superAdmin = ctx.Users.Where(e => e.Email == "superadmin@super.admin").IgnoreQueryFilters().First();

            rolesInitializer.AddAllRolesToUser(superAdmin);

            ctx.SaveChanges();
        }

        private void InsertClientsAndResources()
        {
            configurationDbContext.SaveChanges();

            var identityResources = configurationDbContext.IdentityResources.ToList();
            foreach (var identityResource in Config.IdentityResources)
            {
                if (!identityResources.Any(e => e.Name == identityResource.Name))
                {
                    configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
                }
            }

            var apiResources = configurationDbContext.ApiResources.ToList();
            foreach (var apiResource in Config.ApiResources)
            {
                if (!apiResources.Any(e => e.Name == apiResource.Name))
                {
                    configurationDbContext.ApiResources.Add(apiResource.ToEntity());
                }
            }

            var apiScopes = configurationDbContext.ApiScopes.ToList();
            foreach (var scope in Config.ApiScopes)
            {
                if (!apiScopes.Any(e => e.Name == scope.Name))
                {
                    configurationDbContext.ApiScopes.Add(scope.ToEntity());
                }
                else
                {
                    apiScopes.Where(e => e.Name == scope.Name).First().Enabled = true;
                }
            }

            var clients = configurationDbContext.Clients.Include(e => e.AllowedScopes).ToList();

            foreach (var client in Config.Clients)
            {
                var existingClient = clients
                    .Where(e => e.ClientId == client.ClientId)
                    .FirstOrDefault();

                if (existingClient == null)
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
                else
                {
                    foreach (var scope in client.AllowedScopes)
                    {
                        if (!existingClient.AllowedScopes.Any(e => e.Scope == scope))
                        {
                            existingClient.AllowedScopes.Add(new ClientScope
                            {
                                Scope = scope
                            });
                        }
                    }
                }
            }

            configurationDbContext.SaveChanges();
        }
    }
}


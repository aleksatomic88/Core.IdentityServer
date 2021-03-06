using Core.Users.DAL.Entity;
using Common.Utilities;
using System.Linq;
using Common.Constants;
using System.Collections.Generic;

namespace Core.Users.DAL.Initializers
{
    /// <summary>
    /// Populates db with data that must exist for app to work properly
    /// </summary>
    public class UsersDatabaseInitializer
    {
        private readonly UsersDbContext _ctx;
        private readonly RolesInitializer _rolesInitializer;
        
        public UsersDatabaseInitializer(UsersDbContext ctx,
                                        RolesInitializer rolesInitializer)
        {
            _ctx = ctx;
            _rolesInitializer = rolesInitializer;
        }

        public void Initialize()
        {
            InsertRoles();
            InsertUsers();
            //InsertClientsAndResources();
        }

        private void InsertRoles()
        {
            _rolesInitializer.Initialize();
        }

        private void InsertUsers()
        {
            var superAdminEmail = "super.admin@super.admin";
            var superAdmin = _ctx.Users.FirstOrDefault(e => e.Email == superAdminEmail);

            if (superAdmin == null)
            {
                _ctx.Users.Add(new User
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    PhoneNumber = "00123456789",
                    Email = superAdminEmail,
                    Status = Constants.UserVerificationStatus.Verified,
                    Password = SecurePasswordHasher.Hash("Pass123!"),
                    UserRoles = new List<UserRole>() { new UserRole { Role = _ctx.Roles.First(r => r.Name == RoleConstants.SuperAdminRole) }}
                });;

                _ctx.SaveChanges();
            }
        }

        //private void InsertClientsAndResources()
        //{
        //    configurationDbContext.SaveChanges();

        //    var identityResources = configurationDbContext.IdentityResources.ToList();
        //    foreach (var identityResource in Config.IdentityResources)
        //    {
        //        if (!identityResources.Any(e => e.Name == identityResource.Name))
        //        {
        //            configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
        //        }
        //    }

        //    var apiResources = configurationDbContext.ApiResources.ToList();
        //    foreach (var apiResource in Config.ApiResources)
        //    {
        //        if (!apiResources.Any(e => e.Name == apiResource.Name))
        //        {
        //            configurationDbContext.ApiResources.Add(apiResource.ToEntity());
        //        }
        //    }

        //    var apiScopes = configurationDbContext.ApiScopes.ToList();
        //    foreach (var scope in Config.ApiScopes)
        //    {
        //        if (!apiScopes.Any(e => e.Name == scope.Name))
        //        {
        //            configurationDbContext.ApiScopes.Add(scope.ToEntity());
        //        }
        //        else
        //        {
        //            apiScopes.Where(e => e.Name == scope.Name).First().Enabled = true;
        //        }
        //    }

        //    var clients = configurationDbContext.Clients.Include(e => e.AllowedScopes).ToList();

        //    foreach (var client in Config.Clients)
        //    {
        //        var existingClient = clients
        //            .Where(e => e.ClientId == client.ClientId)
        //            .FirstOrDefault();

        //        if (existingClient == null)
        //        {
        //            configurationDbContext.Clients.Add(client.ToEntity());
        //        }
        //        else
        //        {
        //            foreach (var scope in client.AllowedScopes)
        //            {
        //                if (!existingClient.AllowedScopes.Any(e => e.Scope == scope))
        //                {
        //                    existingClient.AllowedScopes.Add(new ClientScope
        //                    {
        //                        Scope = scope
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    configurationDbContext.SaveChanges();
        //}
    }
}


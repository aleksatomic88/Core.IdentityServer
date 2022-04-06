using Identity.Domain.Initializers;
using Identity.Domain.Model;
using IdentityServer.Utilities;
using Prato.IdentityProvider.Entities;
using System.Linq;

namespace Identity.Domain
{
    /// <summary>
    /// Populets db with some data for testing
    /// </summary>
    public class IdentityDatabaseSeed
    {
        private readonly IdentityDbContext _ctx;

        public IdentityDatabaseSeed(IdentityDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Seed()
        {
            InsertUsers();
        }

        private void InsertUsers()
        {
            var customerEmail = "test@test.test";
            var customerRole = _ctx.Roles.First(r => r.Name == RolesInitializer.CustomerRole);
            var customerUser = _ctx.Users.FirstOrDefault(u => u.Email == customerEmail);

            if (customerUser == null)
            {
                _ctx.Users.Add(new User
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    Name = "Test Customer",
                    EmailConfirmed = true,
                    Deleted = false,
                    Password = SecurePasswordHasher.Hash("Pass123!"),
                    UserRoles = new() { new UserRole { Role = customerRole } }
                });

                _ctx.SaveChanges();
            }
        }
    }
}

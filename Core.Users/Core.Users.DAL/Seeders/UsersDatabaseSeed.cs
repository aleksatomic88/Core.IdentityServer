using Core.Users.DAL.Entity;
using Common.Utilities;
using System.Linq;
using Core.Users.DAL.Initializers;
using Common.Constants;

namespace Core.Users.DAL.Seeders
{
    /// <summary>
    /// Populets db with some data for testing
    /// </summary>
    public class UsersDatabaseSeed
    {
        private readonly UsersDbContext _ctx;

        public UsersDatabaseSeed(UsersDbContext ctx)
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
            var customerRole = _ctx.Roles.First(r => r.Name == Roles.CustomerRole);
            var customerUser = _ctx.Users.FirstOrDefault(u => u.Email == customerEmail);

            if (customerUser == null)
            {
                _ctx.Users.Add(new User
                {
                    FirstName = "Test",
                    LastName = "Customer",
                    Email = customerEmail,
                    Status = Constants.UserVerificationStatus.Verified,
                    Password = SecurePasswordHasher.Hash("Pass123!"),
                    UserRoles = new() { new UserRole { Role = customerRole } }
                });

                _ctx.SaveChanges();
            }
        }
    }
}

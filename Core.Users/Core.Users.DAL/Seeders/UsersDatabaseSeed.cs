using Core.Users.DAL.Entity;
using Common.Utilities;
using System.Linq;
using Common.Constants;
using System.Collections.Generic;

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
            var customerRole = _ctx.Roles.First(r => r.Name == RoleConstants.CustomerRole);
            var customerUser = _ctx.Users.FirstOrDefault(u => u.Email == customerEmail);

            if (customerUser == null)
            {
                _ctx.Users.Add(new User
                {
                    FirstName = "Test",
                    LastName = "Customer",
                    PhoneNumber = "00987654321",
                    Email = customerEmail,
                    Status = Constants.UserVerificationStatus.Verified,
                    Password = SecurePasswordHasher.Hash("Pass123!"),
                    UserRoles = new List<UserRole>() { new UserRole { Role = customerRole } }
                });

                _ctx.SaveChanges();
            }
        }
    }
}

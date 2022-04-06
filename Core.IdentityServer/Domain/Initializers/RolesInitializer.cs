using Identity.Domain.Model;
using Prato.IdentityProvider.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain.Initializers
{
    public class RolesInitializer
    {
        private readonly IdentityDbContext _ctx;

        public RolesInitializer(IdentityDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialize()
        {
            var existingRoles = _ctx.Roles.ToList();

            foreach (var role in Roles)
            {
                var existing = existingRoles.FirstOrDefault(r => r.Id == role.Id);

                if (existing == null)
                {
                    _ctx.Roles.Add(
                        new Role
                        {
                            Id = role.Id,
                            Name = role.Name,
                            Description = role.Description,
                        });
                }
                else
                {
                    existing.Name = role.Name;
                    existing.Description = role.Description;
                }
            }

            _ctx.SaveChanges();
        }

        public void AddAllRolesToUser(User user)
        {
            var roles = _ctx.Roles.ToList();

            var existingUserRoles = _ctx.UserRoles.Where(ur => ur.UserId == user.Id)?.ToList();

            foreach (var role in roles)
            {
                if (!existingUserRoles.Any(ur => ur.RoleId == role.Id))
                {
                    _ctx.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                }
            }
        }

        public static List<Role> Roles => new()
        {
            new Role
            {
                Name = SuperAdminRole,
                Description = "Super Administrator"
            },
            new Role
            {
                Name = AdminRole,
                Description = "Administrator"
            },
            new Role
            {
                Name = CustomerRole,
                Description = "Customer"
            }
        };

        public const string SuperAdminRole = "SuperAdmin";
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";
    }
}

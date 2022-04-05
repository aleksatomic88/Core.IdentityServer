using Identity.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Domain.Initializers
{
    public class RolesInitializer
    {
        private readonly IdentityDbContext ctx;

        public RolesInitializer(IdentityDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void Initialize()
        {
            var existingRoles = ctx.Roles.ToList();

            foreach (var role in Roles)
            {
                var existing = existingRoles.FirstOrDefault(r => r.Id == role.Id);

                if (existing == null)
                {
                    ctx.Roles.Add(
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
                    ((Role)existing).Description = role.Description;
                }
            }

            ctx.SaveChanges();
        }

        public void AddAllRolesToUser(User user)
        {
            var roles = ctx.Roles.ToList();

            var existingUserRoles = ctx.UserRoles.Where(ur => ur.UserId == user.Id)?.ToList();

            foreach (var role in roles)
            {
                if (!existingUserRoles.Any(ur => ur.RoleId == role.Id))
                {
                    ctx.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
                }
            }
        }

        private static List<Role> Roles => new List<Role>
        {
            new Role
            {
                Name = "SuperAdmin",
                Description = "Super Administrator"
            },
            new Role
            {
                Name = "Admin",
                Description = "Administrator"
            },
            new Role
            {
                Name = "Customer",
                Description = "Customer"
            }
        };
    }
}

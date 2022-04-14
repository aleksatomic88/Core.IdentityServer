using Core.Users.DAL.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Core.Users.DAL.Initializers
{
    public class RolesInitializer
    {
        private readonly UsersDbContext _ctx;

        public RolesInitializer(UsersDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialize()
        {
            var existingRoles = _ctx.Roles.ToList();

            foreach (var role in Roles)
            {
                var existing = existingRoles.FirstOrDefault(r => r.Name == role.Name);

                if (existing == null)
                {
                    _ctx.Roles.Add(
                        new Role
                        {
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
            },
            new Role
            {
                Name = ManagerRole,
                Description = "Manager"
            },
            new Role
            {
                Name = EmployeeRole,
                Description = "Employee"
            }
        };

        public const string SuperAdminRole = "super-admin";
        public const string AdminRole = "admin";
        public const string CustomerRole = "customer";
        public const string ManagerRole = "manager";
        public const string EmployeeRole = "employee";
    }
}
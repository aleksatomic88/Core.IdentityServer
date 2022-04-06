using DelegateDecompiler;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Domain.Model
{
    public class User : BaseModel
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }

        public List<UserRole> UserRoles { get; set; }

        [Computed]
        public List<Role> Roles
            => UserRoles != null ? UserRoles.Select(ur => ur.Role).ToList() : new List<Role>();

        [Computed]
        public bool HasRole(string roleName)
            => Roles.Any(r => r.Name == roleName);

    }
}

using DelegateDecompiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Users.Domain
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public List<UserRole> UserRoles { get; set; }

        [NotMapped]
        [Computed]
        public string FullName => FirstName + " " + LastName;

        [NotMapped]
        public List<Role> Roles
            => UserRoles != null ? UserRoles.Select(ur => ur.Role).ToList() : new List<Role>();

        public bool HasRole(string roleName)
            => Roles.Any(r => r.Name == roleName);

    }
}

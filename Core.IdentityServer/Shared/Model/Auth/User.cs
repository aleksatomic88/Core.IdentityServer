using DelegateDecompiler;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Identity.Domain.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public List<Role> UserRoles { get; set; } = new List<Role>();

        [Computed]
        public bool HasRole(string rolename)
        {
            return UserRoles.Any(r => r.Name == rolename);
        }

    }
}

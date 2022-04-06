using DelegateDecompiler;
using EDD.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prato.IdentityProvider.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Identity.Domain.Model
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

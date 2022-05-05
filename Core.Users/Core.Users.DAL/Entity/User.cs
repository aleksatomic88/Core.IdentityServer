using Common.Constants;
using Common.Extensions;
using Core.Users.DAL.Constants;
using DelegateDecompiler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Users.DAL.Entity
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public UserVerificationStatus Status { get; set; }

        public string VerificationToken { get; set; }

        public DateTime VerificationExp { get; set; }

        public string ResetToken { get; set; }

        public DateTime ResetExp { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }

        [NotMapped]
        [Computed]
        public bool IsVerified => Status == UserVerificationStatus.Verified;

        [NotMapped]
        [Computed]
        public string FullName => FirstName + " " + LastName;

        [NotMapped]
        [Computed]
        public bool IsExternal => Roles.First().Name == RoleConstants.CustomerRole; // External Roles

        [NotMapped]
        [Computed]
        public bool IsSuperAdmin => Roles.First().Name == RoleConstants.SuperAdminRole;

        [NotMapped]
        public string StatusDisplay => Status.ToDisplayName();

        [NotMapped]
        [Computed]
        public IEnumerable<Role> Roles => UserRoles.Select(ur => ur.Role).ToList();

        public bool HasRole(string roleName)=> Roles.Any(r => r.Name == roleName);

    }
}

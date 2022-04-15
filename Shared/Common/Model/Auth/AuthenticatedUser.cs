using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Model
{
    public class AuthenticatedUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public bool IsSuperAdmin => Roles.Any(p => p == Constants.Roles.SuperAdminRole);

        public bool IsAdmin => Roles.Any(p => p == Constants.Roles.AdminRole);

        public bool HasRole(string permission)
        {
            return Roles.Any(p => p == permission);
        }

        public bool HasAnyRole(params string[] permissions)
        {
            return Roles.Any(p => permissions.Contains(p));
        }
    }
}


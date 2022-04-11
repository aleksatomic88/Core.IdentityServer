using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Model
{
    public class AuthenticatedUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int? PersonId { get; set; }

        public List<string> PermissionList { get; set; } = new List<string>();

        public string Fullname { get; set; }

        public bool IsAdmin => false; //PermissionList.Any(p => p == Permissions.ADMIN);

        public bool HasPermission(string permission)
        {
            return PermissionList.Any(p => p == permission);
        }

        public bool HasAnyPermission(params string[] permissions)
        {
            return PermissionList.Any(p => permissions.Contains(p));
        }
    }
}


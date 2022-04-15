using Common.Response;
using Core.Users.DAL.Initializers;
using System.Collections.Generic;
using System.Linq;

namespace Core.Users.Service
{
    public class UserResponse : UserBasicResponse
    {
        public List<RoleResponse> Roles { get; set; }

        public bool IsSuperAdmin => Roles.Any(x => x.Name == RolesInitializer.SuperAdminRole);

    }
}

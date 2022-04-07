using System;
using System.Collections.Generic;

namespace Core.Users.Domain.Response
{
    public class UserResponse : BaseResponse
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public List<RoleResponse> Roles { get; set; }

    }
}

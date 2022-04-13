using Core.Users.Service.Response;
using System.Collections.Generic;

namespace Core.Users.Service
{
    public class UserResponse : BaseResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool Verified{ get; set; }

        public List<RoleResponse> Roles { get; set; }

    }
}

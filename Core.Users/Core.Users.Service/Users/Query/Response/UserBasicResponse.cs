using Common.Response;
using Core.Users.DAL.Constants;

namespace Core.Users.Service
{
    public class UserBasicResponse : BaseResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public UserVeificationStatus Status { get; set; }

        public string StatusDisplay { get; set; }

        public bool IsVerified{ get; set; }

    }
}

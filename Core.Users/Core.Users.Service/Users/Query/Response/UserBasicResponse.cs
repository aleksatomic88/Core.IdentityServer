using Common.Response;

namespace Core.Users.Service
{
    public class UserBasicResponse : BaseResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool Verified{ get; set; }

    }
}

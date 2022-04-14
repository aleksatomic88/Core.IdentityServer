using Common.Model.Search;

namespace Core.Users.Service
{
    public class UserQuery : BaseSearchQuery
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool? Verified { get; set; }
    }
}

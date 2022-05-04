namespace User.Functions.Domain.Model
{
    public class UserVerificationEmail
    {

        public string UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string Url { get; set; }

        public NotificationEnum NotificationEnum { get; set; }
    }
}

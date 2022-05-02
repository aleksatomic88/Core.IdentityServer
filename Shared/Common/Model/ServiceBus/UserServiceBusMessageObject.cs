
namespace Common.Model.ServiceBus
{
    public class UserServiceBusMessageObject
    {
        public string UserID { get; set; }
        public string To { get; set; }
        public NotificationEnum Subject { get; set; }
        public string Token { get; set; }
    }
}

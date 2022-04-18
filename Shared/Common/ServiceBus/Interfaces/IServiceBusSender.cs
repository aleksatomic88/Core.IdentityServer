using Common.Model.ServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.ServiceBus.Interfaces
{
    public interface IServiceBusSender
    {
        public Task SendServiceBusMessages(IEnumerable<object> messageObjects);
        public Task SendUserServiceBusMessage(IEnumerable<UserServiceBusMessageObject> messageObjects);
    }
}

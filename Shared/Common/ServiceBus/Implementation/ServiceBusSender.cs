using Common.Model.ServiceBus;
using Common.ServiceBus.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.ServiceBus.Implementation
{
    public class ServiceBusSender : BaseServiceBus, IServiceBusSender
    {
        private readonly IServiceBusConfiguration _serviceBusConfiguration;

        public ServiceBusSender(IServiceBusConfiguration serviceBusConfiguration)
        {
            _serviceBusConfiguration = serviceBusConfiguration;
        }

        public async Task SendServiceBusMessages(IEnumerable<object> messageObjects)
        {
            await using var serviceBusSender = _serviceBusConfiguration.GetServiceBusSender();
            await serviceBusSender.SendMessagesAsync(messageObjects.Select(s => SerializeToServiceBusMessage(s)).ToList());
        }

        public async Task SendUserServiceBusMessage(IEnumerable<UserServiceBusMessageObject> messageObjects)
        {
            await SendServiceBusMessages(messageObjects);
        }
    }
}

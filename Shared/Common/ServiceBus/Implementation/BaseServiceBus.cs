using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Common.ServiceBus.Implementation
{
    public abstract class BaseServiceBus
    {
        protected ServiceBusMessage SerializeToServiceBusMessage(object messageObject)
        {
            var messageString = JsonConvert.SerializeObject(messageObject);
            return new ServiceBusMessage(messageString);
        }
    }
}

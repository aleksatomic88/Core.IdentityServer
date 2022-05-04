using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Common.ServiceBus.Implementation
{
    public abstract class BaseServiceBus
    {
        protected ServiceBusMessage SerializeToServiceBusMessage(object messageObject)
        {
            var messageString = JsonConvert.SerializeObject(messageObject);
            var messageByteArray = Encoding.UTF8.GetBytes(messageString);

            return new ServiceBusMessage(messageByteArray);
        }
    }
}

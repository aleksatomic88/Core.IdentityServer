using Azure.Messaging.ServiceBus;

namespace Common.ServiceBus.Interfaces
{
    public interface IServiceBusConfiguration
    {
         ServiceBusSender GetServiceBusSender();
    }
}
using Azure.Messaging.ServiceBus;
using Common.ServiceBus.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Common.ServiceBus.Configuration
{
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _serviceBusTopic;
        private readonly int _maxRetries;
        private readonly int _delayInSeconds;
        private readonly int _tryTimeoutInSeconds;

        public ServiceBusConfiguration(IConfiguration configuration)
        {
            _serviceBusConnectionString = configuration["ServiceBus:ConnectionSting"];
            _serviceBusTopic = configuration["ServiceBus:Topic"];
            _maxRetries = Convert.ToInt32(configuration["ServiceBus:MaxRetries"]);
            _delayInSeconds = Convert.ToInt32(configuration["ServiceBus:Delay"]);
            _tryTimeoutInSeconds = Convert.ToInt32(configuration["ServiceBus:TryTimeout"]);
        }

        public ServiceBusSender GetServiceBusSender()
        {
            var serviceBusClient = new ServiceBusClient(_serviceBusConnectionString, GetServiceBusClientOptions());
            return serviceBusClient.CreateSender(_serviceBusTopic);
        }

        private ServiceBusClientOptions GetServiceBusClientOptions() =>
        new ServiceBusClientOptions
        {
            RetryOptions = new ServiceBusRetryOptions
            {
                Delay = TimeSpan.FromSeconds(_delayInSeconds),
                TryTimeout = TimeSpan.FromSeconds(_tryTimeoutInSeconds),
                MaxRetries = _maxRetries
            }
        };

    }
}

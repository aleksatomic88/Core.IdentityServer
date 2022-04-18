using Common.ServiceBus.Interfaces;
using Common.ServiceBus.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceBus.Configuration
{
    public static class Configuration
    {
        public static void AddServiceBusServices(this IServiceCollection _services)
        {
            _services.AddScoped<IServiceBusConfiguration, ServiceBusConfiguration>();
            _services.AddScoped<IServiceBusSender, ServiceBusSender>();
        }
    }
}

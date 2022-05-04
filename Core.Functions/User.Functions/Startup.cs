using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using User.Functions.Domain.Interfaces;
using User.Functions.Domain.Services;
using domainMapper = User.Functions.Domain.MapperProfile;

[assembly: FunctionsStartup(typeof(User.Functions.Startup))]

namespace User.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new domainMapper.MapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            builder.Services.AddSingleton(mapper);
            builder.Services.AddTransient<IMailJetService, MailJetService>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }
    }
}
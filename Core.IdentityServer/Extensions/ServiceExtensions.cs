using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Validation;
using IdentityServer.Services;
using Core.Users.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using IdentityServer4;

namespace IdentityServer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<UsersDbContext>();           
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }

        public static IServiceCollection AddIdentityServerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential() // this is for dev only scenarios when you don’t have a certificate to use.
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryApiScopes(Config.GetApiScopes())
                    .AddInMemoryClients(Config.GetClients(configuration.GetSection("ClientApps")))
                    .AddProfileService<ProfileService>();

            return services;
        }

        public static IServiceCollection AddExternalProviders(this IServiceCollection services, IConfiguration configuration)
        {
            var googleConfig = configuration.GetSection("GoogleAuth");

            services.AddAuthentication()
                    .AddGoogle(GoogleDefaults.AuthenticationScheme, "Google Login",
                                                     options =>
                                                     {
                                                         options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                                                         options.ClientId = googleConfig["client_id"];
                                                         options.ClientSecret = googleConfig["client_secret"];
                                                     });

            return services;
        }
    }
}


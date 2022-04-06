using Common.Model;
using Identity.Domain;
using Identity.Domain.Initializers;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Validation;
using IdentityServer.Domain;
using IdentityServer.Services;
using IdentityServer.Domain.Constants;

namespace IdentityServer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IdentityDbContext>();
            services.AddScoped<IDatabaseContext>(x => x.GetRequiredService<IdentityDbContext>());
            services.AddScoped<IdentityDatabaseInitializer>();
            services.AddScoped<IdentityDatabaseSeed>();
            services.AddScoped<RolesInitializer>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }

        public static IServiceCollection AddIdentityService(this IServiceCollection services,
                                                            string connectionString)
        {
            //services.AddIdentity<User, Role>(options =>
            //{
            //    options.SignIn.RequireConfirmedEmail = false;
            //})
            //.AddEntityFrameworkStores<IdentityDbContext>()
            //.AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = DefaultIdentityConstants.PasswordLength;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            });

            return services;
        }

        public static IServiceCollection AddIdentityServerService(this IServiceCollection services,
                                                                  string connectionString)
        {

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var builder = services.AddIdentityServer()
                                  .AddDeveloperSigningCredential() // this is for dev only scenarios when you don’t have a certificate to use.
                                  .AddInMemoryIdentityResources(Config.GetIdentityResources())
                                  .AddInMemoryApiResources(Config.GetApiResources())
                                  .AddInMemoryApiScopes(Config.GetApiScopes())
                                  .AddInMemoryClients(Config.GetClients())
                                  .AddProfileService<ProfileService>();

            return services;
        }

        public static IServiceCollection AddMvcServices(this IServiceCollection services)
        {
            services.AddControllers();
            //services.AddControllers(opt =>
            //{
            //opt.Filters.Add(typeof(GlobalExceptionFilter));
            //opt.Filters.Add(typeof(AuthenticationFilter));
            //opt.Filters.Add(typeof(LanguageFilter));
            //opt.Filters.Add(typeof(LogFilter));
            //})
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            //})
            //.AddFluentValidation(cfg =>
            //{
            //    cfg.ImplicitlyValidateChildProperties = false;
            //});

            return services;
        }

        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            //services.AddSwaggerDocument(settings =>
            //{
            //    settings.SerializerSettings = new JsonSerializerSettings
            //    {
            //        ContractResolver = new CamelCasePropertyNamesContractResolver()
            //    };
            //    settings.Title = "Identity Service";
            //});

            return services;
        }

        public static IServiceCollection AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            return services;
        }
    }
}


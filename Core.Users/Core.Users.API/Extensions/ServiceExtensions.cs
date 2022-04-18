using Microsoft.Extensions.DependencyInjection;
using Core.Users.DAL;
using Core.Users.DAL.Initializers;
using Core.Users.DAL.Seeders;
using Users.Core.Service.Interface;
using Users.Core.Service;
using Core.Users.Service;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.DAL.Repositories.Implementations;
using AutoMapper;
using Users.Core.Service.MapperProfile;
using Common.Interface;
using HashidsNet;

namespace Core.Users.API.Extensions
{
    /// <summary>
    /// Service Extensions
    /// </summary>s
    public static class ServiceExtensions
    {
        /// <summary>
        /// Register Application Services
        /// </summary>s

        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<UsersDbContext>();
            services.AddScoped<IDatabaseContext>(x => x.GetRequiredService<UsersDbContext>());
            services.AddScoped<UsersDatabaseInitializer>();
            services.AddScoped<UsersDatabaseSeed>();
            services.AddScoped<RolesInitializer>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<RegisterUserCommandValidator>();
            services.AddScoped<UpdateUserCommandValidator>();
            services.AddScoped<AuthValidations>();

            return services;
        }

        /// <summary>
        /// Register Reposiories
        /// </summary>s
        public static IServiceCollection RegisterReposiories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        /// <summary>
        /// Register AutoMapper
        /// </summary>s
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            // TODO add tp appSettings
            var saltOptions = new { Salt = "EveryTableHashSalt", MinHashLength = 8 };
            var hashIds = new Hashids(saltOptions.Salt, saltOptions.MinHashLength);
            services.AddSingleton<IHashids>(_ => hashIds);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile(hashIds));
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Extensions;
using Microsoft.Extensions.Configuration;
using Core.Users.DAL;
using HashidsNet;
using System;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment,
                       IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterApplicationServices();

            services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));

            services.AddIdentityServerService(Configuration);

            var hashIdsOptions = Configuration.GetSection("HashIds");
            services.AddSingleton<IHashids>(_ => new Hashids(hashIdsOptions["Salt"], Convert.ToInt32(hashIdsOptions["MinLength"])));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
           app.UseIdentityServer();
        }
    }
}

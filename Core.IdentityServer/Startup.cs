using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Identity.Web.Extensions;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public string ConnectionString { get; }

        public Startup(IWebHostEnvironment environment,
                       IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("IdentityDatabase");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterApplicationServices();

            services.AddIdentityService(ConnectionString);

            services.AddIdentityServerService(ConnectionString);

            services.AddMvcServices();
            services.AddCorsServices();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateDatabase();
        }
    }
}

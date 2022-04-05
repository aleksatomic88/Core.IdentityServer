using Identity.Domain.Initializers;
using Identity.Domain.Model;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Identity.Domain
{
    /// <summary>
    /// Populets db with some data for testing
    /// </summary>
    public class IdentityDatabaseSeed
    {
        private readonly IdentityDbContext ctx;
        private readonly ConfigurationDbContext configurationDbContext;
        private readonly UserManager<User> userManager;
        private readonly RolesInitializer rolesInitializer;

        public IdentityDatabaseSeed(IdentityDbContext ctx,
            UserManager<User> userManager,
            ConfigurationDbContext configurationDbContext,
            RolesInitializer rolesInitializer)
        {
            this.ctx = ctx;
            this.userManager = userManager;
            this.configurationDbContext = configurationDbContext;
            this.rolesInitializer = rolesInitializer;
        }

        public void Seed()
        {
            InsertUsers();
        }

        private void InsertUsers()
        {
            //var email = "coa@test.com";
            var email = "test@test.test";

            var users = ctx.Users.ToList();

            var coaUser = ctx.Users.FirstOrDefault(u => u.Email == email);

            if (coaUser != null)
            {
                rolesInitializer.AddAllRolesToUser(coaUser);
                ctx.SaveChanges();
                return;
            }

            var identityResult = userManager.CreateAsync(new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Name = "Coa Coa"
            }, "Pass123!").Result;

            coaUser = ctx.Users.First(u => u.Email == email);

            rolesInitializer.AddAllRolesToUser(coaUser);

            ctx.SaveChanges();
        }
    }
}


using IdentityServer.Domain;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Utilities;

namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IdentityDbContext _ctx;

        public ResourceOwnerPasswordValidator(IdentityDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == context.UserName);

            if (user != null && user.EmailConfirmed && SecurePasswordHasher.Verify(context.Password, user.Password))
            {
                context.Result = new GrantValidationResult(
                    subject: user.Id.ToString(), // HASH
                    authenticationMethod: "CustomResourceOwnerPassword",
                    claims: new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, user.Email),
                    });
                return;
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}

using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Utilities;
using Core.Users.DAL;
using HashidsNet;

namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UsersDbContext _ctx;
        private readonly IHashids _hashids;

        public ResourceOwnerPasswordValidator(UsersDbContext ctx, 
                                              IHashids hashids)
        {
            _ctx = ctx;
            _hashids = hashids;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == context.UserName);

            if (user != null && user.IsVerified && SecurePasswordHasher.Verify(context.Password, user.Password))
            {
                context.Result = new GrantValidationResult(
                    subject: _hashids.Encode(user.Id),
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

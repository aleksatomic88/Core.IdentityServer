using Core.Users.DAL;
using HashidsNet;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    /// <summary>
    /// The ProfileService class is responsible for adding/deleting claims from the user profile to the access token.
    /// Note: don't forget to add/delete every new claim to the UserClaims property of the Api resource definition in the IdentityServer config (Config.cs),
    /// and to delete the ApiResources table content in the database in order to seed the data properly.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly UsersDbContext _ctx;
        private readonly IHashids _hashids;

        public ProfileService(UsersDbContext ctx, 
                              IHashids hashids)
        {
            _ctx = ctx;
            _hashids = hashids;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = context.Subject.FindFirstValue("sub");
            var scope = context.ValidatedRequest.Raw.Get("scope");

            var user = await _ctx.Users.Include("UserRoles.Role").FirstAsync(x => x.Id == _hashids.DecodeSingle(id));

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, id),
                new Claim(JwtClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(JwtClaimTypes.FamilyName, user.LastName ?? string.Empty),
                new Claim(JwtClaimTypes.Name, user.FullName),                
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Confirmation, user.IsVerified.ToString()),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role.Name));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var id = context.Subject.FindFirstValue("sub");

            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == _hashids.DecodeSingle(id));

            context.IsActive = user?.IsVerified ?? false;
        }
    }
}


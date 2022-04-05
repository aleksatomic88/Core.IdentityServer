using Identity.Domain.Model;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Core.Profiles
{
    /// <summary>
    /// The ProfileService class is responsible for adding/deleting claims from the user profile to the access token.
    /// Note: don't forget to add/delete every new claim to the UserClaims property of the Api resource definition in the IdentityServer config (Config.cs),
    /// and to delete the ApiResources table content in the database in order to seed the data properly.
    /// </summary>
    public class ProfileService : IProfileService
    {
        protected readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.FindFirstValue("sub");
            var scope = context.ValidatedRequest.Raw.Get("scope");

            var user = await _userManager.FindByIdAsync(sub);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),                
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Name, user.UserName ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.FindFirstValue("sub");

            var user = await _userManager.FindByIdAsync(sub);

            context.IsActive = await _userManager.IsEmailConfirmedAsync(user);
        }
    }
}


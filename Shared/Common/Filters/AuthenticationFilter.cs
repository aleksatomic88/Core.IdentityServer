using Common.Interface;
using Common.Model;
using HashidsNet;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Common.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        private readonly IDatabaseContext ctx;
        private readonly IHashids _hashids;

        public AuthenticationFilter(IDatabaseContext ctx, 
                                    IHashids hashids)
        {
            this.ctx = ctx;
            _hashids = hashids;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ctx.CurrentUser = new AuthenticatedUser();

            var name = context.HttpContext.User.Identity.Name;

            if (!string.IsNullOrEmpty(name))
            {
                ctx.CurrentUser.Email = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email).Value; ;
                ctx.CurrentUser.Id = _hashids.DecodeSingle(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id).Value);
                ctx.CurrentUser.FirstName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.GivenName).Value;
                ctx.CurrentUser.LastName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.FamilyName).Value;
                ctx.CurrentUser.FullName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name).Value;

                ctx.CurrentUser.Roles = context.HttpContext.User.Claims
                    .Where(c => c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

                ctx.Token = context.HttpContext.Request.Headers["Authorization"];
            }
        }
    }
}


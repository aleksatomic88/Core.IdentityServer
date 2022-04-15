using Common.Interface;
using Common.Model;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Common.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        private readonly IDatabaseContext ctx;

        public AuthenticationFilter(IDatabaseContext ctx)
        {
            this.ctx = ctx;
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
                ctx.CurrentUser.Id = int.Parse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id).Value);
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


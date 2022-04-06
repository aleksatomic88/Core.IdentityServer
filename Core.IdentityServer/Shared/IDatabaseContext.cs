using IdentityServer.Domain.Model;

namespace Common.Model
{
    public interface IDatabaseContext
    {
        string Token { get; set; }

        AuthenticatedUser CurrentUser { get; set; }
    }
}

using Common.Model.Auth;

namespace Common.Model
{
    public interface IDatabaseContext
    {
        string Token { get; set; }

        AuthenticatedUser CurrentUser { get; set; }
    }
}

using Common.Constants;
using Core.Users.DAL;
using System.Linq;

namespace Core.Users.Service.Command.Users
{
    public class AuthValidations
    {
        private readonly UsersDbContext ctx;

        public AuthValidations(UsersDbContext ctx)
        {
            this.ctx = ctx;
        }

        public bool EmailExists(string email)
        {
            return ctx.Users.Any(e => e.Email == email);
        }

        public bool IsPasswordOk(string password)
        {
            return password.Length >= DefaultIdentityConstants.PasswordLength &&
                password.Any(char.IsDigit) &&
                password.Any(char.IsUpper) &&
                (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation));
        }
    }
}

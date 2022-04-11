using Common.Constants;
using Core.Users.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Users.Service.Command.Users
{
    public class AuthValidations
    {
        private readonly UsersDbContext _ctx;

        public AuthValidations(UsersDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool EmailExists(string email)
        {
            return _ctx.Users.Any(e => e.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _ctx.Users
                                 .Where(u => u.Email == email && !u.Deleted)
                                 .FirstOrDefaultAsync();

            return user != null;
        }

        public bool IsPasswordOk(string password)
        {
            return password.Length >= DefaultIdentityConstants.PasswordLength
                   && password.Any(char.IsDigit)
                   && password.Any(char.IsUpper)
                   && (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation));
        }
    }
}

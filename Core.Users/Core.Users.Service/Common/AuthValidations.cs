using Core.Users.DAL;
using Core.Users.DAL.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Users.Service
{
    public class AuthValidations
    {
        private readonly UsersDbContext _ctx;

        public AuthValidations(UsersDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool UserWithEmailExists(string email)
        {
            return _ctx.Users.Any(e => e.Email == email);
        }

        public async Task<bool> UserWithEmailExistsAsync(string email)
        {
            return await _ctx.Users.AnyAsync(u => u.Email == email);
        }

        public static bool IsPasswordOk(string password)
        {
            return password.Length >= UsersConstants.PasswordLength
                   && password.Any(char.IsDigit)
                   && password.Any(char.IsUpper)
                   && (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation));
        }
    }
}

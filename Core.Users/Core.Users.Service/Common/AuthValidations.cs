using Core.Users.DAL;
using Core.Users.DAL.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<bool> UserWithEmailNotExistsAsync(string email)
        {
            return !await _ctx.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserWithPhoneNumberNotExistsAsync(string phoneNumber)
        {
            return !await _ctx.Users.AnyAsync(u => u.PhoneNumber.Replace("+", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty) ==
                                                     phoneNumber.Replace("+", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty));
        }

        public static bool IsPasswordOk(string password)
        {
            return password.Length >= UsersConstants.PasswordLength
                   && password.Any(char.IsDigit)
                   && password.Any(char.IsUpper)
                   && (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation));
        }
    }

    public static class Extensions
    {
        static readonly List<char> SpecialCharsToRemoveFromPhoneNumber = new() { '+', '(', ')', '-', '/', ' ' };

        public static string RemoveSpecialCharsFromPhoneNumber(this string str)
        {
            SpecialCharsToRemoveFromPhoneNumber.ForEach(c => str = str.Replace(c.ToString(), string.Empty));

            return str;
        }
    }
}

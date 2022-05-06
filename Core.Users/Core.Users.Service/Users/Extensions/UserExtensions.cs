using Common.Utilities;
using Core.Users.DAL.Constants;
using Core.Users.DAL.Entity;
using System;
using System.Collections.Generic;

namespace Core.Users.Service.Users.Extensions
{
    public static class UserExtensions
    {
        public static readonly List<char> SpecialCharsToRemoveFromPhoneNumber = new() { '(', ')', '-', '/', ' ' };

        public static User GenerateVerificationToken(this User user)
        {
            user.VerificationToken = UsersConstants.VerificationTokenPrefix + "_" + Guid.NewGuid().ToString().Replace("-", "");

            user.VerificationExp = DateTime.Now.AddHours(UsersConstants.VerificationTokenValidityDuration);

            return user;
        }

        public static User GenerateResetToken(this User user)
        {
            user.ResetToken = "reset_" + Guid.NewGuid().ToString().Replace("-", "");

            user.ResetExp = DateTime.Now.AddHours(UsersConstants.ResetTokenValidityDuration);

            return user;
        }

        public static User SetPassword(this User user, string password)
        {
            if (!string.IsNullOrEmpty(password))
                user.Password = SecurePasswordHasher.Hash(password);

            return user;
        }        

        public static string ToPhoneNumberWithoutSpecialCharacters(this string phoneNumber)
        {
            SpecialCharsToRemoveFromPhoneNumber.ForEach(c => phoneNumber = phoneNumber.Replace(c.ToString(), string.Empty));

            return phoneNumber.Replace("+", "00");
        }
    }
}

using Core.Users.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Users.Service
{
    public class RegisterUserCommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }

    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly UsersDbContext _ctx;
        private readonly AuthValidations _authValidations;

        public RegisterUserCommandValidator(UsersDbContext ctx,
                                            AuthValidations authValidations,
                                            IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;
            _authValidations = authValidations;

            // RuleFor(cmd => cmd.FirstName).NotEmpty();
            // RuleFor(cmd => cmd.LastName).NotEmpty();
            // RuleFor(cmd => cmd.PhoneNumber).NotEmpty();
            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
            RuleFor(cmd => cmd.Password).NotEmpty();

            RuleFor(cmd => cmd.Password)
                .Must(password => AuthValidations.IsPasswordOk(password))
                .When(cmd => !string.IsNullOrEmpty(cmd.Password))
                .WithMessage(stringLocalizer["PasswordRules"]);                

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => CanRegister(cmd))
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_EmailAlreadyUsed"]);

            RuleFor(cmd => cmd.Roles).NotEmpty();
            RuleFor(cmd => cmd.Roles)
                .Must(roles => roles.Count == 1)
                .When(cmd => cmd.Roles != null && cmd.Roles.Count > 0)
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_SingleRole"]);
        }        

        private async Task<bool> CanRegister(RegisterUserCommand cmd)
        {
           return !await _authValidations.UserWithEmailExistsAsync(cmd.Email);
        }
    }
}

using Core.Users.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Users.Service.Command.Users
{
    public class RegisterUserCommand //  : BaseCommand
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public List<int> Roles { get; set; } = new List<int>();
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

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.UserName).NotEmpty();
            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
            RuleFor(cmd => cmd.PhoneNumber).NotEmpty();
            RuleFor(cmd => cmd.Password).NotEmpty();

            RuleFor(cmd => cmd.Password)
                .Must(password => authValidations.IsPasswordOk(password))
                .When(cmd => !string.IsNullOrEmpty(cmd.Password))
                .WithMessage(stringLocalizer["PasswordRules"]);                

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => CanRegister(cmd))
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_EmailAlreadyUsed"]);

            RuleFor(cmd => cmd.Roles).NotEmpty();
            RuleFor(cmd => cmd.Roles)
                .Must(roles => roles.Count == 1)
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_SingleRole"]);
        }

        private async Task<bool> CanRegister(RegisterUserCommand registerCommand)
        {
           return !await _authValidations.EmailExistsAsync(registerCommand.Email);
        }
    }
}

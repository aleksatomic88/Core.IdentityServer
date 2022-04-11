using Core.Users.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

        public RegisterUserCommandValidator(UsersDbContext ctx,
                                            AuthValidations authValidations)
        {
            this._ctx = ctx;

            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.UserName).NotEmpty();
            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
            RuleFor(cmd => cmd.PhoneNumber).NotEmpty();

            RuleFor(cmd => cmd.Password)
                .Must(password => authValidations.IsPasswordOk(password))
                .When(cmd => !string.IsNullOrEmpty(cmd.Password))
                //.WithMessage(stringLocalizer["PasswordRules"]);
                .WithMessage("PasswordRules");

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => CanRegister(cmd)).WithMessage("InvalidRegisterAttemptEmailAlreadyUsed");
        }

        private async Task<bool> CanRegister(RegisterUserCommand registerCommand)
        {
            var user = await _ctx.Users
                .IgnoreQueryFilters()
                .Where(u => u.UserName == registerCommand.Email)
                .FirstOrDefaultAsync();

            return user == null;
        }
    }
}

using Core.Users.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Core.Users.Service
{
    public class ChangePasswordCommand
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }

    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly UsersDbContext _ctx;
        
        public ChangePasswordValidator(UsersDbContext ctx,
                                       IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;
            
            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
            RuleFor(cmd => cmd.Token).NotEmpty();
            RuleFor(cmd => cmd.Password).NotEmpty();

            RuleFor(cmd => cmd.Password)
                .Must(password => AuthValidations.IsPasswordOk(password))
                .When(cmd => !string.IsNullOrEmpty(cmd.Password))
                .WithMessage(stringLocalizer["PasswordRules"]);

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => Verify(cmd))
                .WithMessage(stringLocalizer["InvalidEmailVerificationAttempt_TokenExpired"]);
        }        

        private async Task<bool> Verify(ChangePasswordCommand cmd)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Email == cmd.Email);

            return user != default && user.ResetToken == cmd.Token && user.ResetExp >= System.DateTime.Now;
        }
    }
}

using Core.Users.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Core.Users.Service
{
    public class EmailVerificationCommand
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }

    public class EmailVerificationValidator : AbstractValidator<EmailVerificationCommand>
    {
        private readonly UsersDbContext _ctx;
        
        public EmailVerificationValidator(UsersDbContext ctx,
                                          IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;
                        
            RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
            RuleFor(cmd => cmd.Token).NotEmpty();

            RuleFor(cmd => cmd)
                .MustAsync((cmd, cancellationToken) => Verify(cmd))
                .WithMessage(stringLocalizer["InvalidEmailVerificationAttempt_TokenExpired"]);
        }        

        private async Task<bool> Verify(EmailVerificationCommand cmd)
        {
            var user = await _ctx.Users.FirstAsync(x => x.Email == cmd.Email);

            return user.VerificationToken == cmd.Token && user.VerificationExp >= System.DateTime.Now;
        }
    }
}

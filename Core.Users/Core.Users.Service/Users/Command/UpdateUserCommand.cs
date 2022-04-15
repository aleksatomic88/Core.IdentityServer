using Core.Users.DAL;
using FluentValidation;
using Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Users.Service
{
    public class UpdateUserCommand //  : BaseCommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public List<int> Roles { get; set; } = new List<int>();
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly UsersDbContext _ctx;
        private readonly AuthValidations _authValidations;

        public UpdateUserCommandValidator(UsersDbContext ctx,
                                          IStringLocalizer<SharedResource> stringLocalizer)
        {
            _ctx = ctx;

            RuleFor(cmd => cmd.FirstName).NotEmpty();
            RuleFor(cmd => cmd.LastName).NotEmpty();
            RuleFor(cmd => cmd.PhoneNumber).NotEmpty();

            RuleFor(cmd => cmd.Roles).NotEmpty();
            RuleFor(cmd => cmd.Roles)
                .Must(roles => roles.Count == 1)
                .When(cmd => cmd.Roles != null && cmd.Roles.Count > 0)
                .WithMessage(stringLocalizer["InvalidRegisterAttempt_SingleRole"]);
        }
    }
}

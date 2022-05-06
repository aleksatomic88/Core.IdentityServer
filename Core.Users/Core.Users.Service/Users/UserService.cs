using AutoMapper;
using Common.Extensions;
using Common.Model;
using Common.Model.ServiceBus;
using Common.ServiceBus.Interfaces;
using Core.Users.DAL;
using Core.Users.DAL.Constants;
using Core.Users.DAL.Entity;
using Core.Users.Service;
using Core.Users.Service.Users.Extensions;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<User, UserResponse, UserBasicResponse, UserQuery>, IUserService
    {

        private readonly RegisterUserCommandValidator _registerUserCmdValidator;
        private readonly UpdateUserCommandValidator _updateUserCmdValidator;
        private readonly EmailVerificationValidator _emailVerificationValidator;
        private readonly ChangePasswordValidator _changePasswordValidator;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IServiceBusSender _serviceBusSender;
        private readonly AuthValidations _authValidations;

        public UserService(UsersDbContext ctx,
                           IMapper mapper,
                           RegisterUserCommandValidator registerUserCommandValidator,
                           UpdateUserCommandValidator updateUserCmdValidator,
                           EmailVerificationValidator emailVerificationValidator,
                           IStringLocalizer<SharedResource> stringLocalizer,
                           IServiceBusSender serviceBusSender,
                           ChangePasswordValidator changePasswordValidator,
                           AuthValidations authValidations)
             : base(ctx,
                    mapper)
        {
            _registerUserCmdValidator = registerUserCommandValidator;
            _updateUserCmdValidator = updateUserCmdValidator;
            _emailVerificationValidator = emailVerificationValidator;
            _stringLocalizer = stringLocalizer;
            _serviceBusSender = serviceBusSender;
            _changePasswordValidator = changePasswordValidator;
            _authValidations = authValidations;
        }

        public async Task<User> Create(RegisterUserCommand cmd)
        {
            _registerUserCmdValidator.ValidateCmd(cmd);
            User user = null;           

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                user = new User
                {
                    FirstName = cmd.FirstName,
                    LastName = cmd.LastName,
                    Email = cmd.Email,
                    PhoneNumber = cmd.PhoneNumber.ToPhoneNumberWithoutSpecialCharacters(),
                    Status = UserVerificationStatus.EmailNotVerified,
                    CreatedById = _ctx.CurrentUser.Id,
                    UpdatedById = _ctx.CurrentUser.Id,
                    UserRoles = new List<UserRole>() { new UserRole { Role = _ctx.Roles.First(r => r.Name == cmd.Roles.First()) } }
                }
                .SetPassword(cmd.Password)
                .GenerateVerificationToken();

                var result = await _ctx.Users.AddAsync(user);

                await _ctx.SaveChangesAsync();
                scope.Complete();
            }

            // TODO extract in method
            if (user != null)
            {
                UserServiceBusMessageObject userServiceBusMessage = _mapper.Map<UserServiceBusMessageObject>(user);
                userServiceBusMessage.NotificationEnum = NotificationEnum.UserCreated;
                await _serviceBusSender.SendServiceBusMessages(new List<UserServiceBusMessageObject>() { userServiceBusMessage });
            }

            return user;
        }

        public async Task<User> Update(int id, UpdateUserCommand cmd)
        {
            _updateUserCmdValidator.ValidateCmd(cmd);

            var phoneNumber = cmd.PhoneNumber.ToPhoneNumberWithoutSpecialCharacters();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = await GetQueryable(Includes()).FirstAsync(x => x.Id == id);

                user.FirstName = user.FirstName == cmd.FirstName ? user.FirstName : cmd.FirstName;
                user.LastName = user.LastName == cmd.LastName ? user.LastName : cmd.LastName;
                user.PhoneNumber = user.PhoneNumber == phoneNumber ? user.PhoneNumber : phoneNumber;

                if (user.Roles.First().Name != cmd.Roles.First())
                {
                    user.Roles.ToList().Clear();
                    user.UserRoles = new List<UserRole>() { new UserRole { Role = _ctx.Roles.First(r => r.Name == cmd.Roles.First()) } };
                }

                await _ctx.SaveChangesAsync();

                scope.Complete();

                return user;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            await CheckIfDeleteUserIsSuperAdmin(id);

            return await base.Delete(id);
        }

        public async Task<bool> QuickValidation(string field, string value)
        {
            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(value))
                throw new ValidationError(new List<ApiError>() { new ApiError(400, _stringLocalizer["BadParams"]) });

            if (field.ToLower().Trim().Contains("email"))
            {
                return await _authValidations.UserWithEmailNotExistsAsync(value);
            }

            if (field.ToLower().Trim().Contains("phone"))
            {
                return await _authValidations.UserWithPhoneNumberNotExistsAsync(value);
            }

            return false;
        }

        public async Task<bool> EmailVerification(EmailVerificationCommand cmd)
        {
            _emailVerificationValidator.ValidateCmd(cmd);

            var user = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == cmd.Email);

            if (user == default) return false;

            user.Status = UserVerificationStatus.Verified;
            user.VerificationExp = System.DateTime.Now;

            user.SetPassword(cmd.Password);

            if (string.IsNullOrEmpty(user.Password))
                throw new ValidationError(new List<ApiError>() { new ApiError(400, _stringLocalizer["CannotValidateUserWithoutPassword"]) });

            await _ctx.SaveChangesAsync();

            return true;
        }

        public async Task<string> ResendEmailVerification(string email)
        {
            var user = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == email);

            if (user == default) return string.Empty;

            user.Status = UserVerificationStatus.EmailNotVerified;
            user.GenerateVerificationToken();

            await _ctx.SaveChangesAsync();

            // TODO extract in method
            if (user != null)
            {
                var userServiceBusMessage = _mapper.Map<UserServiceBusMessageObject>(user);
                userServiceBusMessage.NotificationEnum = NotificationEnum.UserVerificationTokenCreated;
                await _serviceBusSender.SendServiceBusMessages(new List<UserServiceBusMessageObject>() { userServiceBusMessage });
            }

            return user.VerificationToken;
        }

        public async Task<string> ResetPassword(string email)
        {
            var user = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == email);

            if (user == default) return string.Empty;

            user.GenerateResetToken();
            user.Status = UserVerificationStatus.PasswordResetRequested;

            // TODO EMIT User with RESET Token - from Service

            await _ctx.SaveChangesAsync();

            return user.ResetToken;
        }

        public async Task<bool> ChangePassword(ChangePasswordCommand cmd)
        {
            _changePasswordValidator.ValidateCmd(cmd);

            var user = await GetQueryable(Includes()).FirstOrDefaultAsync(x => x.Email == cmd.Email);

            if (user == default) return false;

            user.SetPassword(cmd.Password);

            user.Status = UserVerificationStatus.Verified;
            user.ResetExp = System.DateTime.Now;

            await _ctx.SaveChangesAsync();

            return true;
        }

        protected override string[] Includes()
        {
            return new string[] {
                "UserRoles.Role"
            };
        }

        protected override string[] SearchIncludes()
        {
            return new string[] {
                "UserRoles.Role"
            };
        }

        protected override IQueryable<User> SearchQueryInternal(IQueryable<User> querable, UserQuery searchQuery)
        {
            querable = !string.IsNullOrEmpty(searchQuery.FirstName) ?
                querable.Where(e => e.FirstName.Contains(searchQuery.FirstName)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.LastName) ?
                querable.Where(e => e.LastName.Contains(searchQuery.LastName)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.FullName) ?
                querable.Where(e => e.FullName.Contains(searchQuery.FullName)) : querable;      // Contains uses table scan allways
                //querable.Where(e => e.FullName.StartsWith(searchQuery.FullName)) : querable;  // StartsWith uses index if available
                //querable.Where(e => e.FullName == searchQuery.FullName) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.Email) ?
                querable.Where(e => e.Email.Contains(searchQuery.Email)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.PhoneNumber) ?
                querable.Where(e => e.PhoneNumber.Contains(searchQuery.PhoneNumber)) : querable;
            querable = searchQuery.IsVerified != null ?
                querable.Where(e => e.IsVerified == searchQuery.IsVerified) : querable;

            querable = querable.Where(e => e.IsExternal == searchQuery.IsExternal);

            // remove SeperAdmins from result set
            querable = querable.Where(e => !e.IsSuperAdmin);

            // defult ortder
            querable = querable.OrderByDescending(x => x.Id);

            return querable;
        }

        private async Task CheckIfDeleteUserIsSuperAdmin(int id)
        {
            var user = await Get(id);

            if (user.IsSuperAdmin)
                throw new ValidationError(new List<ApiError>() { new ApiError(400, _stringLocalizer["CannotDeleteSuperAdminUser"]) });
        }
    }
}

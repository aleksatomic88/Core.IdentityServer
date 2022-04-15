using AutoMapper;
using Core.Users.DAL.Entity;
using Common.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Users.Core.Service.Interface;
using Core.Users.Service;
using Common.Extensions;
using Core.Users.DAL;
using Common.Model;
using System.Collections.Generic;
using Core.Users.DAL.Constants;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<User, UserResponse, UserBasicResponse, UserQuery>, IUserService
    {

        private readonly RegisterUserCommandValidator _registerUserCmdValidator;

        public UserService(UsersDbContext ctx,
                           IMapper mapper,
                           RegisterUserCommandValidator registerUserCommandValidator)
             : base(ctx,
                    mapper)
        {
            _registerUserCmdValidator = registerUserCommandValidator;
        }

        public async Task<User> Create(RegisterUserCommand cmd)
        {
            _registerUserCmdValidator.ValidateCmd(cmd);

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = new User
                {
                    FirstName = cmd.FirstName,
                    LastName = cmd.LastName,
                    Email = cmd.Email,
                    PhoneNumber = cmd.PhoneNumber,
                    Status = UserVeificationStatus.NotVerified,
                    Password = SecurePasswordHasher.Hash(cmd.Password),
                    CreatedById = _ctx.CurrentUser.Id,
                    UpdatedById = _ctx.CurrentUser.Id,
                    UserRoles = new() { new UserRole { Role = _ctx.Roles.First(r => r.Id == cmd.Roles.First()) } }
                };

                var result = await _ctx.Users.AddAsync(user);

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

        protected override string[] Includes()
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
            querable = !string.IsNullOrEmpty(searchQuery.FullName)  ?
                querable.Where(e => e.FullName.Contains(searchQuery.FullName)) : querable;      // Contains uses table scan allways
                //querable.Where(e => e.FullName.StartsWith(searchQuery.FullName)) : querable;  // StartsWith uses index if available
                //querable.Where(e => e.FullName == searchQuery.FullName) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.Email) ?
                querable.Where(e => e.Email.Contains(searchQuery.Email)) : querable;
            querable = !string.IsNullOrEmpty(searchQuery.PhoneNumber) ?
                querable.Where(e => e.PhoneNumber.Contains(searchQuery.PhoneNumber)) : querable;
            querable = searchQuery.IsVerified != null ?
                querable.Where(e => e.IsVerified == searchQuery.IsVerified) : querable;

            return querable.OrderByDescending(x => x.Id);
        }
        
        private async Task CheckIfDeleteUserIsSuperAdmin(int id)
        {
            var user = await Get(id);

            if (user.IsSuperAdmin)
                throw new ValidationError(new List<ApiError>() { new ApiError(400, "It is not possible to delete Super Admin user!") });
        }
    }
}

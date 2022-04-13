using AutoMapper;
using Core.Users.Domain;
using Common.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Users.Core.Service.Interface;
using Core.Users.Service;
using Common.Extensions;
using Core.Users.DAL;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<User, UserResponse, UserBasicResponse>, IUserService
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
                    EmailConfirmed = true,
                    PhoneNumber = cmd.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Deleted = false,
                    Password = SecurePasswordHasher.Hash(cmd.Password),
                    UserRoles = new() { new UserRole { Role = _ctx.Roles.First(r => r.Id == cmd.Roles.First()) } }
                };

                var result = await _ctx.Users.AddAsync(user);

                await _ctx.SaveChangesAsync();

                scope.Complete();

                return user;
            }
        }
    }
}

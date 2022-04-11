using AutoMapper;
using Core.Users.DAL;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using Core.Users.Domain.Response;
using Core.Users.Service.Command.Users;
using Common.Extensions;
using Common.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<User, UserResponse>, IUserService
    {

        private readonly RegisterUserCommandValidator _registerUserCmdValidator;

        public UserService(UsersDbContext ctx,
                           IMapper mapper,
                           IGenericRepository<User> userRepository,
                           RegisterUserCommandValidator registerUserCommandValidator)
             : base(ctx,
                    mapper,
                    userRepository)
        {
            _registerUserCmdValidator = registerUserCommandValidator;
        }

        public async Task<User> Create(RegisterUserCommand cmd)
        {
            //var results = _registerUserCmdValidator.Validate(cmd);

            _registerUserCmdValidator.ValidateCmd(cmd);

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = new User
                {
                    Name = cmd.Name,
                    UserName = cmd.UserName,
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

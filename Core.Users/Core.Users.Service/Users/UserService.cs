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

        protected override IQueryable<User> SearchQuery(IQueryable<User> querable, UserQuery searchQuery)
        {
            querable = searchQuery.FirstName != null ? querable.Where(e => e.FirstName.ToLower().Contains(searchQuery.FirstName)) : querable;
            querable = searchQuery.LastName != null ? querable.Where(e => e.LastName.ToLower().Contains(searchQuery.LastName)) : querable;
            querable = searchQuery.Email != null ? querable.Where(e => e.Email.ToLower().Contains(searchQuery.Email)) : querable;
            querable = searchQuery.PhoneNumber != null ? querable.Where(e => e.PhoneNumber.ToLower().Contains(searchQuery.FirstName)) : querable;
            // querable = searchQuery.Verified != null ? querable.Where(e => e.Verified == searchQuery.Verified) : querable;

            return querable.OrderByDescending(x => x.Id);
        }
    }
}

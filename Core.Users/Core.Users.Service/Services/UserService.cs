using AutoMapper;
using Core.Users.DAL;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using Core.Users.Domain.Response;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<User, UserResponse>
    {
        public UserService(UsersDbContext ctx,
                           IMapper mapper,
                           IGenericRepository<User> userRepository)
             : base(ctx,
                    mapper,
                    userRepository)
        {
        }
        public async Task<UserResponse> Get(int id)
        {
            var result = await _ctx.Users
                                   .Include("UserRoles.Role")
                                   .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<UserResponse>(result);
        }
    }
}

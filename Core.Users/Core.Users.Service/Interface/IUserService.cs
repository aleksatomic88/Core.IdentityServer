using Core.Users.DAL.Entity;
using Core.Users.Service;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IUserService : IBaseService<User, UserResponse, UserBasicResponse, UserQuery>
    {
        Task<User> Create(RegisterUserCommand cmd);
    }
}

using Core.Users.Service.Command.Users;
using Core.Users.Domain.Model;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IUserService
    {
        Task<User> Create(RegisterUserCommand cmd);
    }
}

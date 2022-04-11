using Core.Users.Domain.Command.User;
using Core.Users.Domain.Model;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IUserService
    {
        Task<User> Create(RegisterUserCommand);
    }
}

using Core.Users.Service;
using Core.Users.Domain;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IUserService
    {
        Task<User> Create(RegisterUserCommand cmd);
    }
}

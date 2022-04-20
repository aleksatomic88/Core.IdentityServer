using Core.Users.DAL.Entity;
using Core.Users.Service;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IUserService : IBaseService<User, UserResponse, UserBasicResponse, UserQuery>
    {

        Task<User> Update(int id, UpdateUserCommand cmd);

        Task<User> Create(RegisterUserCommand cmd);

        Task<bool> QuickValidation(string field, string value);

        Task<bool> EmailVerification(EmailVerificationCommand cmd);

        Task<string> ResendEmailVerification(string email);
             
        Task<string> ResetPassword(string email);

        Task<bool> ChangePassword(ChangePasswordCommand cmd);
    }
}

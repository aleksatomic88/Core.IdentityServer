using AutoMapper;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using Core.Users.Domain.Response;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<User, UserResponse>, IUserService
    {
        public UserService(IMapper mapper,
                           // IUnitOfWork unitOfWork,
                           IGenericRepository<User> userRepository)
            : base(mapper,
                   // unitOfWork,
                   userRepository)
        {
        }
    }
}

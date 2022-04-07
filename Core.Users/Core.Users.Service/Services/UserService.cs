using AutoMapper;
using Core.Users.DAL.Entities;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public sealed class UserService : BaseService<WeatherForecasts, UserResponse>, IUserService
    {
        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper,
                   unitOfWork,
                   unitOfWork.WeatherForecastsRepository)
        {
        }
    }
}

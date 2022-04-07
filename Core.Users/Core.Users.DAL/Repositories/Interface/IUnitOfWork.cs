using Core.Users.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<WeatherForecasts> WeatherForecastsRepository { get;}
        IUserRepositoryCustom  WeatherForecastsRepositoryCustom { get; }
        Task<bool> Complete();
    }
}

using Core.Users.DAL.Entities;
using Core.Users.DAL.Repositories.Interface;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreUsersDbContext _ctx;

        public UnitOfWork(CoreUsersDbContext ctx)
        {
            _ctx = ctx;
        }

        private readonly IGenericRepository<WeatherForecasts> _weatherForecastsRepository;
        public IGenericRepository<WeatherForecasts> WeatherForecastsRepository
            => _weatherForecastsRepository ?? new GenericRepository<WeatherForecasts>(_ctx);

        private readonly IUserRepositoryCustom _weatherForecastsRepositoryCustom;
        public IUserRepositoryCustom WeatherForecastsRepositoryCustom
            => _weatherForecastsRepositoryCustom ?? new UserForecastsRepositoryCustom(_ctx);

        public async Task<bool> Complete()
            => await _ctx.SaveChangesAsync() > 0;

        public void Dispose()
            => _ctx.Dispose();
    }
}

using Core.Users.DAL.Entities;
using Core.Users.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Implementations
{
    public class UserForecastsRepositoryCustom : GenericRepository<WeatherForecasts>, IUserRepositoryCustom
    {
        public UserForecastsRepositoryCustom(CoreUsersDbContext context)
             : base(context)
        {

        }

        public async Task<IEnumerable<WeatherForecasts>> GetCustom(string name)
            => await _context.WeatherForecasts.Where(x => x.Summary == name).ToListAsync();
    }
}

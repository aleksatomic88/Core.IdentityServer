using Core.Users.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Interface
{
    public interface IUserRepositoryCustom : IGenericRepository<WeatherForecasts> 
    {
        Task<IEnumerable<WeatherForecasts>> GetCustom(string name); 
    }
}

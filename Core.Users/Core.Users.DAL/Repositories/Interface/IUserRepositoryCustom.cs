using Core.Users.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Interface
{
    public interface IUserRepositoryCustom : IGenericRepository<User> 
    {
        Task<IEnumerable<User>> GetAllWithRoles(); 
    }
}

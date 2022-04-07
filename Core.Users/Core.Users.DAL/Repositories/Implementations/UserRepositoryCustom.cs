using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Implementations
{
    public class UserRepositoryCustom : GenericRepository<User>, IUserRepositoryCustom
    {
        public UserRepositoryCustom(CoreUsersDbContext context)
             : base(context)
        {

        }

        public async Task<IEnumerable<User>> GetCustom(string name)
            => await _context.Users.Where(x => x.UserName == name).ToListAsync();
    }
}

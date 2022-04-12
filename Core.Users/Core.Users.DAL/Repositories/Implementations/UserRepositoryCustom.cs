using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Implementations
{
    public class UserRepositoryCustom : GenericRepository<User>, IUserRepositoryCustom
    {
        public UserRepositoryCustom(UsersDbContext context)
             : base(context)
        {

        }

        public async Task<IEnumerable<User>> GetAllWithRoles()
            => await _context.Users
                             .Include("UserRoles.Role")
                             .ToListAsync();
    }
}

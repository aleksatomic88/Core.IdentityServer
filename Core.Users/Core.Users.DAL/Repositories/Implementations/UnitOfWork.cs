using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersDbContext _ctx;

        public UnitOfWork(UsersDbContext ctx)
        {
            _ctx = ctx;
        }

        private readonly IGenericRepository<User> _userRepository;
        public IGenericRepository<User> UserRepository
            => _userRepository ?? new GenericRepository<User>(_ctx);

        private readonly IUserRepositoryCustom _userRepositoryCustom;
        public IUserRepositoryCustom UserRepositoryCustom
            => _userRepositoryCustom ?? new UserRepositoryCustom(_ctx);

        public async Task<bool> Complete()
            => await _ctx.SaveChangesAsync() > 0;

        public void Dispose()
            => _ctx.Dispose();
    }
}

using Core.Users.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get;}
        IUserRepositoryCustom  UserRepositoryCustom { get; }
        Task<bool> Complete();
    }
}

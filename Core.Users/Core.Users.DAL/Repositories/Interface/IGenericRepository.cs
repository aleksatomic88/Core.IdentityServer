using Core.Users.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Interface
{
    public interface IGenericRepository<T>
       where T : BaseEntity
    {
        Task<T> Get(int id, string[] includes = default);
        Task<T> Find(Expression<Func<T, bool>> predicate, string[] includes = default);

        Task<IEnumerable<T>> GetAll(string[] includes = default);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, string[] includes = default);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}

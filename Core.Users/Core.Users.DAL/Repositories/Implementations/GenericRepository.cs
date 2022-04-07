using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Users.DAL.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : BaseEntity
    {
        protected readonly UsersDbContext _context;

        public GenericRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<T> Get(int id, string[] includes = default)
            => await GetQuery(includes).FirstOrDefaultAsync(x => x.Id == id);
        

        public async Task<T> Find(Expression<Func<T, bool>> predicate, string[] includes = default)
            => await GetQuery(includes).FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> GetAll(string[] includes = default)
            => await GetQuery(includes).ToListAsync();  

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, string[] includes = default)
            => await GetQuery(includes).Where(predicate).ToListAsync();

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        private IQueryable<T> GetQuery(string[] includes = default)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes == default)
                return query;

            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }
    }
}

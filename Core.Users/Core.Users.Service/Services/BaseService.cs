using AutoMapper;
using Core.Users.DAL;
using Users.Core.Service.Interface;
using Core.Users.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Users.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Common.Model.Search;
using Core.Users.Service.Response;

namespace Users.Core.Service
{
    public abstract class BaseService<T, TResponse, TBasicResponse, TQuery> : IBaseService<T, TResponse, TBasicResponse, TQuery>
        where T :  BaseEntity
        where TResponse : BaseResponse
        where TBasicResponse : BaseResponse
        where TQuery : BaseQuery
    {
        protected readonly UsersDbContext _ctx;
        protected readonly IMapper _mapper;

        public const int PageSize = 10;

        public BaseService(UsersDbContext ctx,
                           IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<TResponse> Get(int id, string[] includes = default)
        {
            var result = await GetQueryable(includes).FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<TResponse>(result);            
        }

        public async Task<SearchResponse<TBasicResponse>> Search(TQuery searchQuery, string[] includes = default)
        {
            var pageSize = searchQuery.PageSize <= 0 ? PageSize : searchQuery.PageSize;
            var pageNumber = searchQuery.PageNumber <= 0 ? 1 : searchQuery.PageNumber;


            var entities = await SearchQuery(GetQueryable(includes), searchQuery)
                                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)                                    
                                    .ToListAsync();

            var dtos = _mapper.Map<List<TBasicResponse>>(entities);

            return new SearchResponse<TBasicResponse>
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalCount = dtos.Count,
                Result = dtos                
            };
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _ctx.Set<T>().FindAsync(id);

            if (!CanDeleteEntity(entity))
                return false;           

            entity.Deleted = true;

            await _ctx.SaveChangesAsync();

            return true;
        }

        protected IQueryable<T> GetQueryable(string[] includes = default)
        {
            var queryable = _ctx.Set<T>().AsQueryable();

            if (includes == default)
                return queryable;

            foreach (var include in includes)
                queryable = queryable.Include(include);

            return queryable;
        }

        protected IQueryable<T> SearchQuery(IQueryable<T> queryable, TQuery searchQuery)
        {
            queryable = SearchQueryInternal(queryable, searchQuery);

            queryable = queryable.Where(x => !x.Deleted);

            queryable = queryable.OrderByDescending(x => x.Id);

            return queryable;
        }

        protected abstract IQueryable<T> SearchQueryInternal(IQueryable<T> queryable, TQuery searchQuery);

        
        protected virtual bool CanDeleteEntity(T entity)
        {
            // for example we should not be able delete SuperAdmin User
            return true;
        }
    }
}

using AutoMapper;
using Core.Users.DAL;
using Users.Core.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Users.DAL.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Common.Model.Search;
using Common.Response;
using DelegateDecompiler.EntityFrameworkCore;

namespace Users.Core.Service
{
    public abstract class BaseService<T, TResponse, TBasicResponse, TSearchQuery> : IBaseService<T, TResponse, TBasicResponse, TSearchQuery>
        where T :  BaseEntity
        where TResponse : BaseResponse
        where TBasicResponse : BaseResponse
        where TSearchQuery : BaseSearchQuery
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

        public virtual async Task<TResponse> Get(int id)
        {
            var entity = await GetQueryable(Includes())
                                .DecompileAsync()
                                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<TResponse>(entity);            
        }

        public async Task<SearchResponse<TBasicResponse>> Search(TSearchQuery searchQuery)
        {
            var pageSize = searchQuery.PageSize <= 0 ? PageSize : searchQuery.PageSize;
            var pageNumber = searchQuery.PageNumber <= 0 ? 1 : searchQuery.PageNumber;

            var querable = SearchQuery(GetQueryable(SearchIncludes()), searchQuery).DecompileAsync();

            var entities = await querable.Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            return new SearchResponse<TBasicResponse>
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalCount = await querable.CountAsync(),
                Result = _mapper.Map<List<TBasicResponse>>(entities)
            };
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await _ctx.Set<T>().FindAsync(id);

            _ctx.Remove(entity);

            await _ctx.SaveChangesAsync();

            return true;
        }

        protected virtual string[] Includes()
        {
            return default;
        }

        protected virtual string[] SearchIncludes()
        {
            return default;
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

        protected IQueryable<T> SearchQuery(IQueryable<T> queryable, TSearchQuery searchQuery)
        {
            queryable = SearchQueryInternal(queryable, searchQuery);

            queryable = queryable.OrderByDescending(x => x.Id);

            return queryable;
        }

        protected abstract IQueryable<T> SearchQueryInternal(IQueryable<T> queryable, TSearchQuery searchQuery);

    }
}

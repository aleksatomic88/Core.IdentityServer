using AutoMapper;
using Core.Users.DAL;
using Users.Core.Service.Interface;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Users.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Users.Core.Service
{
    public abstract class BaseService<T, TResponse, TBasicResponse> : IBaseService<T, TResponse, TBasicResponse>
        where T : BaseEntity
        where TResponse : BaseResponse
        where TBasicResponse : BaseResponse
    {
        protected readonly UsersDbContext _ctx;
        protected readonly IMapper _mapper;

        public BaseService(UsersDbContext ctx,
                           IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<TResponse> Get(int id, string[] includes = default)
        {
            var result = await GetQuery(includes).FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<TResponse>(result);
        }

        public async Task<List<TBasicResponse>> GetAll(string[] includes = default)
        {
            var result = await GetQuery(includes).ToListAsync();

            return _mapper.Map<List<TBasicResponse>>(result);
        }

        private IQueryable<T> GetQuery(string[] includes = default)
        {
            var query = _ctx.Set<T>().AsQueryable();

            if (includes == default)
                return query;

            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }
    }
}

using AutoMapper;
using Core.Users.DAL;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain.Model;
using Core.Users.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public abstract class BaseService<T, TResponse> : IBaseService<T, TResponse>
        where T :  BaseEntity
        where TResponse : BaseResponse
    {
        protected readonly UsersDbContext _ctx;
        protected readonly IMapper _mapper;
        protected readonly IGenericRepository<T> _repository;

        public BaseService(UsersDbContext ctx,
                           IMapper mapper,
                           IGenericRepository<T> repository)
        {
            _ctx = ctx;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<TResponse> Get(int id, string[] includes = default)
        {
            var result = await _repository.Get(id, includes);

            return _mapper.Map<TResponse>(result);
        }

        public async Task<List<TResponse>> GetAll(string[] includes = default)
        {
            var result = await _repository.GetAll(includes);

            return _mapper.Map<List<TResponse>>(result);
        }
    }
}

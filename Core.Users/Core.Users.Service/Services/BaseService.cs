using AutoMapper;
using Core.Users.DAL.Entities;
using Core.Users.DAL.Repositories.Interface;
using Core.Users.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Core.Service.Interface;

namespace Users.Core.Service
{
    public abstract class BaseService<T, TResponse> : IBaseService<T, TResponse>
        where T :  BaseEntity
        where TResponse : BaseResponse
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<T> _repository;

        public BaseService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<TResponse> Get(int id)
        {
            var result = await _repository.Get(id);

            return _mapper.Map<TResponse>(result);
        }

        public async Task<List<TResponse>> GetAll()
        {
            var result = await _repository.GetAll();

            return _mapper.Map<List<TResponse>>(result);
        }
    }
}

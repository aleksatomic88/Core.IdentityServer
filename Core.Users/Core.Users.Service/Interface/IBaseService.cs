using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IBaseService<T, TResponse>
    {
        Task<TResponse> Get(int id);

        Task<List<TResponse>> GetAll();
    }
}

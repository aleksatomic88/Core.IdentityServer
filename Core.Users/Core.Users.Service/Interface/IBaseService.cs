using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IBaseService<T, TResponse>
    {
        Task<TResponse> Get(int id, string[] includes = default);

        Task<List<TResponse>> GetAll(string[] includes = default);
    }
}

using Common.Model.Search;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IBaseService<T, TResponse, TBasicResponse, TQuery>
    {
        Task<TResponse> Get(int id, string[] includes = default);

        Task<SearchResponse<TBasicResponse>> Search(TQuery searchQuery, string[] includes = default);
    }
}

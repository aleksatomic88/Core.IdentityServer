using Common.Model.Search;
using System.Threading.Tasks;

namespace Users.Core.Service.Interface
{
    public interface IBaseService<T, TResponse, TBasicResponse, TSearchQuery>
    {
        Task<TResponse> Get(int id, string[] includes = default);

        Task<SearchResponse<TBasicResponse>> Search(TSearchQuery searchQuery, string[] includes = default);

        Task<bool> Delete(int id);
    }
}

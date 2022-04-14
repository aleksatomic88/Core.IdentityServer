
namespace Common.Model.Search
{
    public class BaseSearchQuery
    {
        public int? Id { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }
}

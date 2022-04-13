using System;
using System.Collections.Generic;

namespace Common.Model.Search
{
    public class SearchResponse<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / (double)PageSize);

        public int TotalCount { get; set; }

        public List<T> Result { get; set; }
    }
}

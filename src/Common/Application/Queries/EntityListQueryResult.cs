using System.Collections.Generic;

namespace SiteWatchman.Common.Application.Queries
{
    public class EntityListQueryResult<T>
    {
        public IEnumerable<T> DataList { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
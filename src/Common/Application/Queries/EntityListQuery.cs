
using System;
using System.Linq.Expressions;
using SiteWatchman.Common.Domain;

namespace SiteWatchman.Common.Application.Queries
{
    public class EntityListQuery<T> where T : Entity
    {
        public int Page { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }

        public Expression<Func<T, bool>> Where { get; set; }
        protected EntityListQuery()
        {
            // setup the default values if the api client doesn't specify them
            SortBy = "CreatedOnUtc";
            Page = 1;
            PageSize = 25;
        }
    }
}
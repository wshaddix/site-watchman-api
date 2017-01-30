using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SiteWatchman.Common.Application.Queries;
using SiteWatchman.Common.Domain;

namespace SiteWatchman.Common.Persistance
{
    public interface IDatabase
    {
        Task DeleteAsync<T>(string id) where T : Entity;

        T Get<T>(Expression<Func<T, bool>> query) where T : Entity;

        Task<T> GetByIdAsync<T>(string id) where T : Entity;

        EntityListQueryResult<T> List<T>(Expression<Func<T, bool>> where, string sortBy, int page, int pageSize) where T : Entity;

        Task SaveAsync(object data);
    }
}
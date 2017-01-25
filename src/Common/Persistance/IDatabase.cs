using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SiteWatchman.Common.Application.Queries;
using SiteWatchman.Common.Domain;

namespace SiteWatchman.Common.Persistance
{
    public interface IDatabase
    {
        /// <summary>
        /// Deletes an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>Nothing</returns>
        Task DeleteAsync<T>(string id) where T : Entity;

        /// <summary>
        /// Returns a single item from the database based on a WHERE expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns>An instance of T</returns>
        T Get<T>(Expression<Func<T, bool>> query) where T : Entity;

        /// <summary>
        /// Returns a sorted list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns>a Tuple that contains the total count of items, the total number of pages (based on query.PageSize) and the list
        /// of items</returns>
        EntityListQueryResult<T> List<T>(EntityListQuery<T> query) where T : Entity;

        /// <summary>
        /// Inserts a new record or updates an existing record in the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SaveAsync(object data);
    }
}
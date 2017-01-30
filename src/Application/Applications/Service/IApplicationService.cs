using System.Threading.Tasks;
using SiteWatchman.Application.Applications.Commands;
using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Services;
using SiteWatchman.Common.Application.Queries;

namespace SiteWatchman.Application.Applications.Service
{
    public interface IApplicationService : IService
    {
        Task<ApplicationModel> CreateAsync(ApplicationCreateCommand cmd);
        Task<ApplicationModel> UpdateAsync(ApplicationUpdateCommand cmd);
        Task DeleteAsync(ApplicationDeleteCommand cmd);
        ApplicationModel GetById(string id);
        ApplicationModel GetByName(string name);
        EntityListQueryResult<ApplicationModel> List(string criteria, string sortBy, int page, int pageSize);
    }
}
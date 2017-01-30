using System.Threading.Tasks;
using SiteWatchman.Application.Shared.Services;
using SiteWatchman.Application.Users.Commands;
using SiteWatchman.Application.Users.Models;
using SiteWatchman.Common.Application.Queries;

namespace SiteWatchman.Application.Users.Service
{
    public interface IUserService : IService
    {
        Task<UserModel> CreateAsync(UserCreateCommand cmd);
        Task<UserModel> UpdateAsync(UserUpdateCommand cmd);
        Task DeleteAsync(UserDeleteCommand cmd);
        UserModel GetById(string id);
        UserModel GetByUsername(string username);

        UserModel GetByEmail(string email);
        EntityListQueryResult<UserModel> List(string criteria, string sortBy, int page, int pageSize);
    }
}
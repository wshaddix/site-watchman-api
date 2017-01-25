using System.Threading.Tasks;
using SiteWatchman.Application.Platform.Models;

namespace SiteWatchman.Application.Platform.Commands.CreatePlatform
{
    public interface ICreatePlatformCommand
    {
        Task<PlatformModel> ExecuteAsync(  string platformAdminUsername,
                                string platformAdminPassword,
                                string applicationName,
                                string applicationApiKey);
    }
}
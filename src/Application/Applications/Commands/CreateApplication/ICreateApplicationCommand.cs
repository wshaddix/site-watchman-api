using System.Threading.Tasks;
using SiteWatchman.Application.Applications.Models;

namespace SiteWatchman.Application.Applications.Commands.CreateApplication
{
    public interface ICreateApplicationCommand
    {
        Task<ApplicationModel> ExecuteAsync(string applicationName, string apiKey=null);
    }
}
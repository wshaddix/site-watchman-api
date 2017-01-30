using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Shared.Services
{
    public interface IService
    {
        MessageContext MessageContext { get; set; }
    }
}
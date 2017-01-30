using SiteWatchman.Application.Shared.Models;

namespace SiteWatchman.Application.Applications.Models
{
    public class ApplicationModel : ModelBase
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}
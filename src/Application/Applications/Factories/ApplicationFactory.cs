using SiteWatchman.Application.Applications.Models;

namespace SiteWatchman.Application.Applications.Factories
{
    internal class ApplicationFactory
    {
        internal static ApplicationModel ConvertToModel(Domain.Applications.Application application)
        {
            if (application == null) return null;

            return new ApplicationModel
            {
                Name = application.Name,
                ApiKey = application.ApiKey
            };
        }
    }
}
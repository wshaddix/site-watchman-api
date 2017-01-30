using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Models;

namespace SiteWatchman.Application.Applications.Factories
{
    internal class ApplicationFactory : ModelFactoryBase
    {
        internal static ApplicationModel ConvertToModel(Domain.Application data)
        {
            if (null == data) return null;

            var model = MapBaseProperties<ApplicationModel>(data);

            model.Name = data.Name;
            model.ApiKey = data.ApiKey;

            return model;
        }
    }
}
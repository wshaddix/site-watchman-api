using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Users.Models;

namespace SiteWatchman.Application.Platform.Models
{
    public sealed class PlatformModel
    {
        public ApplicationModel App { get; }
        public UserModel User { get; }

        public PlatformModel(ApplicationModel app, UserModel user)
        {
            App = app;
            User = user;
        }
    }
}
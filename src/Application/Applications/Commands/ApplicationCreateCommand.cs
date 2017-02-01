using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Commands
{
    public class ApplicationCreateCommand : MessageBase<ApplicationCreateCommand, ApplicationCreateCommandValidator>
    {
        public string Name;
        public string ApiKey;
    }
}
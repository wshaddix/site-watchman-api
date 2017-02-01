using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Commands
{
    public class ApplicationUpdateCommand : MessageBase<ApplicationUpdateCommand, ApplicationUpdateCommandValidator>
    {
        public string Id;
        public string Name;
        public string ApiKey;
        public bool ResetApiKey { get; set; }
    }
}
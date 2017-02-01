using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Commands
{
    public class ApplicationDeleteCommand : MessageBase<ApplicationDeleteCommand, ApplicationDeleteCommandValidator>
    {
        public string Id { get; private set; }

        public ApplicationDeleteCommand(string id)
        {
            Id = id;
        }
    }
}
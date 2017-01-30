using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserDeleteCommand : Message<UserDeleteCommand, UserDeleteCommandValidator>
    {
        public string Id;
    }
}
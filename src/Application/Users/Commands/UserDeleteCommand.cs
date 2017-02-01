using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserDeleteCommand : MessageBase<UserDeleteCommand, UserDeleteCommandValidator>
    {
        public string Id;
    }
}
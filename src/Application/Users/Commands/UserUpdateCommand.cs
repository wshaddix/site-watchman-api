using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserUpdateCommand : Message<UserUpdateCommand, UserUpdateCommandValidator>
    {
        public string Id;
        public string Username;
        public string FirstName;
        public string Email;
    }
}
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserCreateCommand : MessageBase<UserCreateCommand, UserCreateCommandValidator>
    {
        public string Username;
        public string Email;
        public string Password;
        public string FirstName;
    }
}
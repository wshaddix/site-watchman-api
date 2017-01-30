using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Users.Commands
{
    public class UserCreateCommand : Message<UserCreateCommand, UserCreateCommandValidator>
    {
        public string Username;
        public string Email;
        public string Password;
        public string FirstName;
    }
}
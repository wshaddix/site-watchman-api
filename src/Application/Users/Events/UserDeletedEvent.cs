using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;
using SiteWatchman.Application.Users.Models;

namespace SiteWatchman.Application.Users.Events
{
    public sealed class UserDeletedEvent : DomainEventBase
    {
        public UserModel User { get; set; }

        public UserDeletedEvent(UserModel user, MessageContext context) : base(context)
        {
            User = user;
            Message = $"{MessageContext.Username} deleted the user named {User.FirstName}";
        }
    }
}
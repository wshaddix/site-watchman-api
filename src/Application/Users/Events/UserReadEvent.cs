using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;
using SiteWatchman.Application.Users.Models;

namespace SiteWatchman.Application.Users.Events
{
    public sealed class UserReadEvent : DomainEventBase
    {
        public UserModel User { get; set; }

        public UserReadEvent(UserModel user, MessageContext context) : base(context)
        {
            User = user;
            Message = $"{MessageContext.Username} read the user named {User.FirstName}";
        }
    }
}
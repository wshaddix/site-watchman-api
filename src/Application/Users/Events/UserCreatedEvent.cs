using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;
using SiteWatchman.Application.Users.Models;

namespace SiteWatchman.Application.Users.Events
{
    public sealed class UserCreatedEvent : DomainEventBase
    {
        public UserModel User { get; set; }

        public UserCreatedEvent(UserModel application, MessageContext context) : base(context)
        {
            User = application;
            Message = $"{MessageContext.Username} created a new user named {User.FirstName}";
        }
    }
}
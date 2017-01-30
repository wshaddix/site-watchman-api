using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Users.Events
{
    public class UserListedEvent : DomainEventBase
    {
        public UserListedEvent(MessageContext context) : base(context)
        {
        }
    }
}
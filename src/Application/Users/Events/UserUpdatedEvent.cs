using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;
using SiteWatchman.Application.Users.Models;

namespace SiteWatchman.Application.Users.Events
{
    public sealed class UserUpdatedEvent : DomainEventBase
    {
        public UserModel OriginalUser { get; set; }
        public UserModel UpdatedUser { get; set; }

        public UserUpdatedEvent(MessageContext context) : base(context)
        {
        }
    }
}
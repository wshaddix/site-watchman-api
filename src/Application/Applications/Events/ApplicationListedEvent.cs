using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Events
{
    public sealed class ApplicationListedEvent : DomainEventBase
    {
        public ApplicationListedEvent(MessageContext context) : base(context)
        {
        }
    }
}
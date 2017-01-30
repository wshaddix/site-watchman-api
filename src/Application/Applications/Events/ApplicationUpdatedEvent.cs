using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Events
{
    public sealed class ApplicationUpdatedEvent : DomainEventBase
    {
        public ApplicationModel OriginalApplication { get; set; }
        public ApplicationModel UpdatedApplication { get; set; }

        public ApplicationUpdatedEvent(MessageContext context) : base(context)
        {
        }
    }
}
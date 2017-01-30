using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Events
{
    public sealed class ApplicationCreatedEvent : DomainEventBase
    {
        public ApplicationModel Application { get; set; }

        public ApplicationCreatedEvent(ApplicationModel application, MessageContext context) : base(context)
        {
            Application = application;
            Message = $"{MessageContext.Username} created a new application named {Application.Name}";
        }
    }
}
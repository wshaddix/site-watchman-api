using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Events
{
    public sealed class ApplicationDeletedEvent : DomainEventBase
    {
        public ApplicationModel Application { get; set; }

        public ApplicationDeletedEvent(ApplicationModel application, MessageContext context) : base(context)
        {
            Application = application;
            Message = $"{MessageContext.Username} deleted the application named {Application.Name}";
        }
    }
}
using SiteWatchman.Application.Applications.Models;
using SiteWatchman.Application.Shared.Events;
using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Applications.Events
{
    public sealed class ApplicationReadEvent : DomainEventBase
    {
        public ApplicationModel Application { get; set; }

        public ApplicationReadEvent(ApplicationModel application, MessageContext context) : base(context)
        {
            Application = application;
            Message = $"{MessageContext.Username} read the application named {Application.Name}";
        }
    }
}
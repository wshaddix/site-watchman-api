using SiteWatchman.Application.Shared.Messages;

namespace SiteWatchman.Application.Shared.Events
{
    public abstract class DomainEventBase
    {
        public MessageContext MessageContext { get; set; }

        public string Message { get; set; }
        public string Name => GetType().Name;

        protected DomainEventBase(MessageContext messageContext)
        {
            MessageContext = messageContext;
        }
    }
}
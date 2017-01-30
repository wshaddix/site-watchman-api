using System;

namespace SiteWatchman.Application.Shared.Messages
{
    public class MessageContext
    {
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreatedOnUtc { get; private set; }
        public string Environment { get; set; }
        public string HostName { get; set; }
        public string Region { get; set; }
        public string TenantName { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string UserSessionId { get; set; }
        public string Version { get; set; }

        public MessageContext()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }
    }
}
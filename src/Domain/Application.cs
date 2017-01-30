using System;
using SiteWatchman.Common.Domain;

namespace SiteWatchman.Domain
{
    public class Application : Entity
    {
        private Application(){}
        public Application(string name, string apiKey, string createdByUserId)
        {
            Name = name;
            ApiKey = apiKey;
            CreatedByUserId = createdByUserId;
        }

        public void Delete(string deletedByUserId)
        {
            IsDeleted       = true;
            DeletedByUserId = deletedByUserId;
            DeletedOnUtc    = DateTime.UtcNow;
        }

        public void Update(string name, bool resetApiKey, string updatedByUserId)
        {
            UpdatedByUserId = updatedByUserId;
            UpdatedOnUtc    = DateTime.UtcNow;
            Name            = name;

            if (resetApiKey)
            {
                ApiKey = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            }
        }

        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}
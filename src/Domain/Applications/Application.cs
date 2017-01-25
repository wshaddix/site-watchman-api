using SiteWatchman.Common.Domain;

namespace SiteWatchman.Domain.Applications
{
    public class Application : Entity
    {
        public Application(string name, string apiKey)
        {
            Name = name;
            ApiKey = apiKey;
        }

        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}
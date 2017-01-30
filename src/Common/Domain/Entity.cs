using System;

namespace SiteWatchman.Common.Domain
{
    /// <summary>
    /// Base class for all Entity objects. The property accessors on this base class MUST remain internal and not made
    /// private or else the database won't be able to populate these fields when converting from a JObject to T
    /// (Azure DocumentDb)
    /// </summary>
    public abstract class Entity
    {
        public  string CreatedByUserId { get; set; }
        public  DateTime CreatedOnUtc { get; set; }
        public  string DeletedByUserId { get; set; }
        public  DateTime? DeletedOnUtc { get; set; }
        public string EntityType => GetType().Name;
        public  string Id { get; set; }
        public  bool IsActive { get; set; }
        public  bool IsDeleted { get; set; }
        public  string UpdatedByUserId { get; set; }
        public  DateTime? UpdatedOnUtc { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedOnUtc = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
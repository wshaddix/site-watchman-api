using System;

namespace SiteWatchman.Common.Domain
{
    /// <summary>
    /// Base class for all Entity objects. The property accessors on this base class MUST remain internal and not made private or else the database
    /// won't be able to populate these fields when converting from a JObject to T (Azure DocumentDb)
    /// </summary>
    public abstract class Entity
    {
        internal string CreatedBy { get; set; }
        internal DateTime CreatedOnUtc { get; set; }
        internal string DeletedBy { get; set; }
        internal DateTime? DeletedOnUtc { get; set; }
        public string EntityType => GetType().Name;
        internal string Id { get; set; }
        internal bool IsActive { get; set; }
        internal bool IsDeleted { get; set; }
        internal string UpdatedBy { get; set; }
        internal DateTime? UpdatedOnUtc { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedOnUtc = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
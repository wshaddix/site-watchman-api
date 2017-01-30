using System;

namespace SiteWatchman.Application.Shared.Models
{
    public abstract class ModelBase
    {
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
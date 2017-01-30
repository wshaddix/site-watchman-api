using SiteWatchman.Common.Domain;

namespace SiteWatchman.Application.Shared.Models
{
    internal abstract class ModelFactoryBase
    {
        protected static TModel MapBaseProperties<TModel>(Entity entity) where TModel : ModelBase, new()
        {
            if (null == entity) return null;

            var model = new TModel
            {
                CreatedByUserId = entity.CreatedByUserId,
                CreatedOnUtc    = entity.CreatedOnUtc,
                DeletedByUserId = entity.DeletedByUserId,
                DeletedOnUtc    = entity.DeletedOnUtc,
                Id              = entity.Id,
                IsActive        = entity.IsActive,
                IsDeleted       = entity.IsDeleted,
                UpdatedByUserId = entity.UpdatedByUserId,
                UpdatedOnUtc    = entity.UpdatedOnUtc
            };

            return model;
        }
    }
}
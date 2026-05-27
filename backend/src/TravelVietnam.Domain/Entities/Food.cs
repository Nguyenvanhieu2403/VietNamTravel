using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Food : BaseAuditableEntity
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? RecipeLink { get; set; }
        public string? ThumbnailUrl { get; set; }

        // Navigation properties
        public virtual Province Province { get; set; } = null!;
    }
}

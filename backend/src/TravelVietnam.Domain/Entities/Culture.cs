using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Culture : BaseAuditableEntity
    {
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? BannerUrl { get; set; }
        public int? RegionId { get; set; }
        public string? CultureType { get; set; }
        public string? FestivalSeason { get; set; }
        public bool IsFeatured { get; set; }

        // Navigation properties
        public virtual Region? Region { get; set; }
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
    }
}

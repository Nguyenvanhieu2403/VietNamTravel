using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Destination : BaseAuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? BannerUrl { get; set; }
        public int ProvinceId { get; set; }
        public int RegionId { get; set; }
        public string? Category { get; set; }
        public string? BestTimeToVisit { get; set; }
        public decimal? EstimatedBudget { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? Rating { get; set; }
        public bool IsFeatured { get; set; }

        // Navigation properties
        public virtual Province Province { get; set; } = null!;
        public virtual Region Region { get; set; } = null!;
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<TravelPlanDestination> TravelPlanDestinations { get; set; } = new List<TravelPlanDestination>();
    }
}

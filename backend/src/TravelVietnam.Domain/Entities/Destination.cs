using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Destination : BaseAuditableEntity
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal EntryFee { get; set; }
        public string? Slug { get; set; }
        public string? ThumbnailUrl { get; set; }

        // Navigation properties
        public virtual Province Province { get; set; } = null!;
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<TravelPlanDestination> TravelPlanDestinations { get; set; } = new List<TravelPlanDestination>();
    }
}

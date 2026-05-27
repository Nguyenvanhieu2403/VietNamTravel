using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Province : BaseAuditableEntity
    {
        public int RegionId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? CultureDescription { get; set; }
        public string? BestTimeToVisit { get; set; }
        public decimal AverageBudget { get; set; }
        public string? VideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }

        // Navigation properties
        public virtual Region Region { get; set; } = null!;
        public virtual ICollection<Destination> Destinations { get; set; } = new List<Destination>();
        public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
        public virtual ICollection<Festival> Festivals { get; set; } = new List<Festival>();
        public virtual ICollection<TravelSeason> Seasons { get; set; } = new List<TravelSeason>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
    }
}

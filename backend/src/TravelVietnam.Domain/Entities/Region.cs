using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Region : BaseAuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
        public virtual ICollection<Destination> Destinations { get; set; } = new List<Destination>();
        public virtual ICollection<Culture> Cultures { get; set; } = new List<Culture>();
    }
}

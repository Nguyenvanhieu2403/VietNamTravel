using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class MediaFile : BaseAuditableEntity
    {
        public string Url { get; set; } = null!;
        public string FileType { get; set; } = null!; // Image, Video
        
        // Dynamic target associations
        public int? ProvinceId { get; set; }
        public int? DestinationId { get; set; }
        public int? BlogId { get; set; }

        // Navigation properties
        public virtual Province? Province { get; set; }
        public virtual Destination? Destination { get; set; }
        public virtual Blog? Blog { get; set; }
    }
}

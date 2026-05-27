using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Review : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public int? DestinationId { get; set; }
        public int? ProvinceId { get; set; }
        public int Rating { get; set; } // 1 - 5
        public string? Comment { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Destination? Destination { get; set; }
        public virtual Province? Province { get; set; }
    }
}

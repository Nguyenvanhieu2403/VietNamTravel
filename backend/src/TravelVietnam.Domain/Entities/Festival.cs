using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class Festival : BaseAuditableEntity
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? HeldDate { get; set; }
        public string? LunarDate { get; set; }

        // Navigation properties
        public virtual Province Province { get; set; } = null!;
    }
}

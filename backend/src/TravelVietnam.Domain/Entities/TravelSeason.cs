using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class TravelSeason : BaseAuditableEntity
    {
        public int ProvinceId { get; set; }
        public string SeasonName { get; set; } = null!; // e.g. Spring, Summer, Autumn, Winter, Dry, Rainy
        public string? Months { get; set; }           // e.g. "1,2,3"
        public string? WeatherCondition { get; set; }
        public string? Tips { get; set; }

        // Navigation properties
        public virtual Province Province { get; set; } = null!;
    }
}

using System.Collections.Generic;
using TravelVietnam.Domain.Common;

namespace TravelVietnam.Domain.Entities
{
    public class TravelPlan : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Budget { get; set; }
        public int DurationDays { get; set; }
        public string? Season { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<TravelPlanDestination> TravelPlanDestinations { get; set; } = new List<TravelPlanDestination>();
    }
}

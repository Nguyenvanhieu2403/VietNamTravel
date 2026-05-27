namespace TravelVietnam.Domain.Entities
{
    public class TravelPlanDestination
    {
        public int TravelPlanId { get; set; }
        public int DestinationId { get; set; }
        public int VisitOrder { get; set; }

        // Navigation properties
        public virtual TravelPlan TravelPlan { get; set; } = null!;
        public virtual Destination Destination { get; set; } = null!;
    }
}

using MediatR;
using TravelVietnam.Application.DTOs.Travel;

namespace TravelVietnam.Application.Features.Regions.Queries
{
    public class GetRegionBySlugQuery : IRequest<RegionDto?>
    {
        public string Slug { get; set; } = null!;
    }
}

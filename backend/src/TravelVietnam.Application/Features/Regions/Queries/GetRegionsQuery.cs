using MediatR;
using TravelVietnam.Application.DTOs.Travel;

namespace TravelVietnam.Application.Features.Regions.Queries
{
    public class GetRegionsQuery : IRequest<List<RegionDto>>
    {
    }
}

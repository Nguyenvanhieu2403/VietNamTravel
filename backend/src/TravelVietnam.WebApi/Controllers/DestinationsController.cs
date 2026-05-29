using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Features.Destinations.Queries;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class DestinationsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<DestinationDto>>> Get([FromQuery] GetDestinationsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<DestinationDto>> GetBySlug(string slug)
        {
            var result = await Mediator.Send(new GetDestinationBySlugQuery { Slug = slug });
            if (result == null)
            {
                return NotFound(new { Message = $"Không tìm thấy điểm đến '{slug}'." });
            }
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<DestinationDto>>> GetFeatured([FromQuery] int limit = 10)
        {
            return await Mediator.Send(new GetFeaturedDestinationsQuery { Limit = limit });
        }

        [HttpGet("by-region/{regionId}")]
        public async Task<ActionResult<PaginatedList<DestinationDto>>> GetByRegion(int regionId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await Mediator.Send(new GetDestinationsByRegionQuery { RegionId = regionId, PageNumber = pageNumber, PageSize = pageSize });
        }

        [HttpGet("by-province/{provinceId}")]
        public async Task<ActionResult<PaginatedList<DestinationDto>>> GetByProvince(int provinceId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await Mediator.Send(new GetDestinationsByProvinceQuery { ProvinceId = provinceId, PageNumber = pageNumber, PageSize = pageSize });
        }
    }
}

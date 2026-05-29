using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Features.Cultures.Queries;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class CulturesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<CultureDto>>> Get([FromQuery] GetCulturesQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<CultureDto>> GetBySlug(string slug)
        {
            var result = await Mediator.Send(new GetCultureBySlugQuery { Slug = slug });
            if (result == null)
            {
                return NotFound(new { Message = $"Không tìm thấy văn hóa '{slug}'." });
            }
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<CultureDto>>> GetFeatured([FromQuery] int limit = 10)
        {
            return await Mediator.Send(new GetFeaturedCulturesQuery { Limit = limit });
        }

        [HttpGet("by-region/{regionId}")]
        public async Task<ActionResult<PaginatedList<CultureDto>>> GetByRegion(int regionId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await Mediator.Send(new GetCulturesByRegionQuery { RegionId = regionId, PageNumber = pageNumber, PageSize = pageSize });
        }
    }
}

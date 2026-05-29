using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Features.Regions.Queries;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class RegionsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<RegionDto>>> Get()
        {
            return await Mediator.Send(new GetRegionsQuery());
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<RegionDto>> GetBySlug(string slug)
        {
            var result = await Mediator.Send(new GetRegionBySlugQuery { Slug = slug });
            if (result == null)
            {
                return NotFound(new { Message = $"Không tìm thấy thông tin cho vùng '{slug}'." });
            }
            return Ok(result);
        }
    }
}

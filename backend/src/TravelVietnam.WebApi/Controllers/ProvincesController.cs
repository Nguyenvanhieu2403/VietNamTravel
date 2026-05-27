using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Features.Provinces.Commands;
using TravelVietnam.Application.Features.Provinces.Queries;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class ProvincesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProvinceListDto>>> Get([FromQuery] GetProvincesQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<ProvinceDto>> GetBySlug(string slug)
        {
            var result = await Mediator.Send(new GetProvinceBySlugQuery { Slug = slug });
            if (result == null)
            {
                return NotFound(new { Message = $"Không tìm thấy thông tin cho tỉnh/thành '{slug}'." });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] CreateProvinceCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetBySlug), new { slug = command.Slug, version = "1.0" }, id);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Features.Blogs.Queries;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class BlogsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<BlogDto>>> Get([FromQuery] GetBlogsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<BlogDto>> GetBySlug(string slug)
        {
            var result = await Mediator.Send(new GetBlogBySlugQuery { Slug = slug });
            if (result == null)
            {
                return NotFound(new { Message = $"Không tìm thấy bài viết '{slug}'." });
            }
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<BlogDto>>> GetFeatured([FromQuery] int limit = 10)
        {
            return await Mediator.Send(new GetFeaturedBlogsQuery { Limit = limit });
        }

        [HttpGet("latest")]
        public async Task<ActionResult<List<BlogDto>>> GetLatest([FromQuery] int limit = 10)
        {
            return await Mediator.Send(new GetLatestBlogsQuery { Limit = limit });
        }
    }
}

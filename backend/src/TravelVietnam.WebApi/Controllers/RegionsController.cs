using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Features.Regions.Commands;
using TravelVietnam.Application.Features.Regions.Queries;

namespace TravelVietnam.WebApi.Controllers;

[ApiVersion("1.0")]
public class RegionsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] string? searchTerm = null)
    {
        var query = new GetRegionsQuery { PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm };
        var result = await Mediator.Send(query);
        return Success(result, "Regions retrieved successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var query = new GetRegionByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return Success(result, "Region retrieved successfully");
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug)
    {
        var query = new GetRegionBySlugQuery { Slug = slug };
        var result = await Mediator.Send(query);
        return Success(result, "Region retrieved successfully");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRegionCommand command)
    {
        var result = await Mediator.Send(command);
        return Created($"/api/v1/regions/{result.Id}", new { data = result, success = true, message = "Region created successfully" });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRegionCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return Success(result, "Region updated successfully");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var command = new DeleteRegionCommand { Id = id };
        await Mediator.Send(command);
        return Success(true, "Region deleted successfully");
    }
}

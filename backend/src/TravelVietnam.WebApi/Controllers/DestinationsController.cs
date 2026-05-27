using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Features.Destinations.Commands;
using TravelVietnam.Application.Features.Destinations.Queries;

namespace TravelVietnam.WebApi.Controllers;

[ApiVersion("1.0")]
public class DestinationsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] int? provinceId = null, [FromQuery] string? searchTerm = null)
    {
        var query = new GetDestinationsQuery { PageNumber = pageNumber, PageSize = pageSize, ProvinceId = provinceId, SearchTerm = searchTerm };
        var result = await Mediator.Send(query);
        return Success(result, "Destinations retrieved successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var query = new GetDestinationByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return Success(result, "Destination retrieved successfully");
    }

    [HttpGet("{id}/related")]
    public async Task<IActionResult> GetRelated([FromRoute] int id, [FromQuery] int limit = 5)
    {
        var query = new GetRelatedDestinationsQuery { DestinationId = id, Limit = limit };
        var result = await Mediator.Send(query);
        return Success(result, "Related destinations retrieved successfully");
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> Create([FromBody] CreateDestinationCommand command)
    {
        var result = await Mediator.Send(command);
        return Created($"/api/v1/destinations/{result.Id}", new { data = result, success = true, message = "Destination created successfully" });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDestinationCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return Success(result, "Destination updated successfully");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var command = new DeleteDestinationCommand { Id = id };
        await Mediator.Send(command);
        return Success(true, "Destination deleted successfully");
    }
}

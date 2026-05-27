using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TravelVietnam.Application.Common.Models;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private ISender? _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected OkObjectResult Success<T>(T data, string message = "Success")
        {
            var response = new ApiResponse<T> { Data = data, Message = message };
            return Ok(response);
        }

        protected OkObjectResult SuccessList<T>(List<T> items, int totalCount, int pageNumber, int pageSize, string message = "Success")
        {
            var paginatedData = new PaginatedResponse<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var response = new ApiResponse<PaginatedResponse<T>> { Data = paginatedData, Message = message };
            return Ok(response);
        }

        protected NotFoundObjectResult NotFound(string message = "Resource not found")
        {
            var response = new ErrorResponse
            {
                Errors = new List<ErrorDetail> { new() { Code = "NOT_FOUND", Message = message } }
            };
            return NotFound(response);
        }

        protected BadRequestObjectResult BadRequest(string message = "Bad request")
        {
            var response = new ErrorResponse
            {
                Errors = new List<ErrorDetail> { new() { Code = "BAD_REQUEST", Message = message } }
            };
            return BadRequest(response);
        }
    }
}

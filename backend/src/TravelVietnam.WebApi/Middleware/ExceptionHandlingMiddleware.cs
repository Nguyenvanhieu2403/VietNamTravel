using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TravelVietnam.Application.Common.Exceptions;
using TravelVietnam.Application.Common.Models;
using ValidationException = TravelVietnam.Application.Common.Exceptions.ValidationException;

namespace TravelVietnam.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Errors = validationEx.Errors
                        .SelectMany(kvp => kvp.Value.Select(msg => new ErrorDetail
                        {
                            Code = "VALIDATION_ERROR",
                            Message = msg,
                            Field = kvp.Key
                        }))
                        .ToList();
                    break;

                case NotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Errors.Add(new ErrorDetail
                    {
                        Code = notFoundEx.Code ?? "NOT_FOUND",
                        Message = notFoundEx.Message
                    });
                    break;

                case ForbiddenException forbiddenEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response.Errors.Add(new ErrorDetail
                    {
                        Code = "FORBIDDEN",
                        Message = forbiddenEx.Message
                    });
                    break;

                case UnauthorizedException unauthorizedEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Errors.Add(new ErrorDetail
                    {
                        Code = "UNAUTHORIZED",
                        Message = unauthorizedEx.Message
                    });
                    break;

                case ConflictException conflictEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Errors.Add(new ErrorDetail
                    {
                        Code = "CONFLICT",
                        Message = conflictEx.Message
                    });
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Errors.Add(new ErrorDetail
                    {
                        Code = "INTERNAL_ERROR",
                        Message = _env.IsDevelopment() ? exception.Message : "An error occurred while processing your request"
                    });
                    break;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(json);
        }
    }
}

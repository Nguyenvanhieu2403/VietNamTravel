using MediatR;
using Serilog;

namespace TravelVietnam.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior()
    {
        _logger = Log.ForContext<LoggingBehavior<TRequest, TResponse>>();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.Information("Handling {RequestName}", requestName);

        try
        {
            var response = await next();
            _logger.Information("Completed {RequestName} successfully", requestName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Request {RequestName} failed with exception", requestName);
            throw;
        }
    }
}

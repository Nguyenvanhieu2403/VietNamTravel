using FluentValidation;
using MediatR;
using TravelVietnam.Application.Common.Exceptions;
using ValidationException = TravelVietnam.Application.Common.Exceptions.ValidationException;

namespace TravelVietnam.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.Select(failure => failure.ErrorMessage).ToArray()
            );

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
}

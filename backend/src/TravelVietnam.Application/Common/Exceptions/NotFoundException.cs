namespace TravelVietnam.Application.Common.Exceptions;

public class NotFoundException : ApplicationException
{
    public string? Code { get; set; }

    public NotFoundException(string message, string? code = null) : base(message)
    {
        Code = code ?? "NOT_FOUND";
    }

    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} with identifier {key} was not found.")
    {
        Code = "NOT_FOUND";
    }
}

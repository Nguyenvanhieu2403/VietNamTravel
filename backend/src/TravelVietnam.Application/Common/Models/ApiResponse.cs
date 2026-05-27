namespace TravelVietnam.Application.Common.Models;

public class ApiResponse<T>
{
    public required T Data { get; set; }
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class PaginatedResponse<T>
{
    public required List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class ErrorResponse
{
    public List<ErrorDetail> Errors { get; set; } = new();
    public bool Success { get; set; } = false;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ErrorDetail
{
    public required string Code { get; set; }
    public required string Message { get; set; }
    public string? Field { get; set; }
}

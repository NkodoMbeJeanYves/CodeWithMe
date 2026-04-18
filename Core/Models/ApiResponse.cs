namespace CodeWithMe.Core.Models
{
    internal sealed record ApiResponse(string Code, string Message, string TraceId);
    internal sealed record pagedResponse<T>(int PageNumber, int pageSize, int TotalItems, int TotalPages, IEnumerable<T> Data);
}

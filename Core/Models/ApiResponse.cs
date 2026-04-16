namespace CodeWithMe.Core.Models
{
    internal sealed record ApiResponse(string Code, string Message, string TraceId);

    internal sealed record ValidationReport(string propertyName, string errorMessage);
}

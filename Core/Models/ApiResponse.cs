using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodeWithMe.Core.Models
{
    internal sealed record ApiResponse(string Code, string Message, Dictionary<string, string[]> Errors, string TraceId);

    internal sealed record ValidationReport(string, propertyName, string errorMessage);
}

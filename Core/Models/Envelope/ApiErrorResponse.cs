namespace CodeWithMe.Core.Models.Envelope;

public sealed record ApiErrorResponse(
    IReadOnlyList<ApiError> Errors,
    string TraceId,
    DateTime Timestamp,
    string Path
);

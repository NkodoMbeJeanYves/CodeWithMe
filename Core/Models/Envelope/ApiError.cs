namespace CodeWithMe.Core.Models.Envelope;

public sealed record ApiError(
    string Code,
    string Message,
    string? Field = null,
    object? Details = null
);

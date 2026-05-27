namespace CodeWithMe.Core.Models.Envelope;

public sealed record PageMeta(
    int Page,
    int Size,
    long TotalItems,
    int TotalPages,
    string? Sort = null
);

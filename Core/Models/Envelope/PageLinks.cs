namespace CodeWithMe.Core.Models.Envelope;

public sealed record PageLinks(
    string Self,
    string? Next,
    string? Prev,
    string First,
    string Last
);

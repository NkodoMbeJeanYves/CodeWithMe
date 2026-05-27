namespace CodeWithMe.Core.Exceptions;

public sealed class ConflictException : DomainException
{
    public ConflictException(string message, string? field = null, object? details = null)
        : base("CONFLICT", 409, message, field, details) { }
}

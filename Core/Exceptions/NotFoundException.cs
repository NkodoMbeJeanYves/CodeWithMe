namespace CodeWithMe.Core.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message = "Resource not found.", string? field = null)
        : base("NOT_FOUND", 404, message, field) { }

    public static NotFoundException For(string resource, string id)
        => new($"{resource} '{id}' not found.");
}

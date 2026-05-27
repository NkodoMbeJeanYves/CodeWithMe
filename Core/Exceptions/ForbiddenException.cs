namespace CodeWithMe.Core.Exceptions;

public sealed class ForbiddenException : DomainException
{
    public ForbiddenException(string message = "Access denied.")
        : base("FORBIDDEN", 403, message) { }
}

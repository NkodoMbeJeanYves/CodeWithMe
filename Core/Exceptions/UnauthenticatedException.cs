namespace CodeWithMe.Core.Exceptions;

public sealed class UnauthenticatedException : DomainException
{
    public UnauthenticatedException(string message = "Authentication required.", string code = "UNAUTHENTICATED")
        : base(code, 401, message) { }
}

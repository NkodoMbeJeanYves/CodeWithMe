namespace CodeWithMe.Core.Exceptions;

public sealed class TenantMismatchException : DomainException
{
    public TenantMismatchException(string message = "Tenant header does not match JWT tenant.")
        : base("TENANT_MISMATCH", 403, message) { }
}

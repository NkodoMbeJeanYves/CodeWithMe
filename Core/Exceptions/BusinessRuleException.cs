namespace CodeWithMe.Core.Exceptions;

public sealed class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message, string? field = null, object? details = null)
        : base("BUSINESS_RULE_VIOLATION", 422, message, field, details) { }
}

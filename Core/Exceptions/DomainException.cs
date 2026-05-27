namespace CodeWithMe.Core.Exceptions;

/// <summary>
/// Base des exceptions métier mappées par <see cref="Middlewares.GlobalExceptionHandler"/>
/// vers le format d'erreur RFC 7807 simplifié du contrat (errors[], traceId, timestamp, path).
/// </summary>
public abstract class DomainException : Exception
{
    public string Code { get; }
    public int StatusCode { get; }
    public string? Field { get; }
    public object? Details { get; }

    protected DomainException(
        string code,
        int statusCode,
        string message,
        string? field = null,
        object? details = null,
        Exception? inner = null) : base(message, inner)
    {
        Code = code;
        StatusCode = statusCode;
        Field = field;
        Details = details;
    }
}

namespace CodeWithMe.Core.Exceptions;

/// <summary>
/// Transition d'état métier interdite (ex. session déjà annulée, note déjà publiée).
/// Contrat section 6 : 409 STATE_INVALID.
/// </summary>
public sealed class StateInvalidException : DomainException
{
    public StateInvalidException(string message, object? details = null)
        : base("STATE_INVALID", 409, message, details: details) { }
}

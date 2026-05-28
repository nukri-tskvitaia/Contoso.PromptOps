namespace Contoso.PromptOps.Domain.Exceptions;

public sealed class DomainException(string message) : Exception(message);
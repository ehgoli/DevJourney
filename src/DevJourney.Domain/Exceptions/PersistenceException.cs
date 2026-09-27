namespace DevJourney.Domain.Exceptions;

public sealed class PersistenceException : Exception
{
    public PersistenceErrorType ErrorType { get; }

    public PersistenceException(
        string message,
        PersistenceErrorType errorType,
        Exception innerException)
        : base(message, innerException)
    {
        ErrorType = errorType;
    }
}
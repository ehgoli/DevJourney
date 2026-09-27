namespace DevJourney.Domain.Exceptions;

public enum PersistenceErrorType
{
    DuplicateData,
    ConstraintViolation,
    ConcurrencyConflict,
    TransientFailure,
    Unknown
}
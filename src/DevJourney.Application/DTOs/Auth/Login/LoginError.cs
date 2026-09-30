namespace DevJourney.Application.DTOs.Auth.Login;

public enum LoginError
{
    NotFound,
    InvalidCredentials,
    UserSuspended,
    PendingActivation,
    TooManyAttempts
}
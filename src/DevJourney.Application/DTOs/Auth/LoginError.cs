namespace DevJourney.Application.DTOs.Auth;

public enum LoginError
{
    NotFound,
    InvalidCredentials,
    UserSuspended,
    PendingActivation
}
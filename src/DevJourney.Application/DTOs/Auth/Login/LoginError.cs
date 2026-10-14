namespace DevJourney.Application.DTOs.Auth.Login;

public enum LoginError
{
    NotFound,
    InvalidCredentials,
    UserSuspended,
    TooManyAttempts,
    EmailVerificationRequired,
    PhoneVerificationRequired
}
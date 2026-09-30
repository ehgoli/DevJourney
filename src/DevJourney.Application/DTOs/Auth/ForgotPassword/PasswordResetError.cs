namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public enum PasswordResetError
{
    NotFound,
    UserSuspended,
    InvalidCode,
    Expired,
    TooManyAttempts,
    ResendTooSoon,
    InvalidToken,
    Unexpected
}
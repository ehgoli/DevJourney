namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetResponse(
    bool IsSuccess,
    PasswordResetError? Error)
{
    public static PasswordResetResponse CodeSent()
        => new(true, null);

    public static PasswordResetResponse NotFound()
        => new(false, PasswordResetError.NotFound);

    public static PasswordResetResponse ResendTooSoon()
        => new(false, PasswordResetError.ResendTooSoon);
    
    public static PasswordResetResponse UserSuspended()
        => new(false, PasswordResetError.UserSuspended);

    public static PasswordResetResponse TooManyAttempts()
        => new(false, PasswordResetError.TooManyAttempts);
}
namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetResendCodeResponse(
    bool Success,
    PasswordResetError? Error)
{
    public static PasswordResetResendCodeResponse CodeSent(
        int resendAvailableInSeconds)
        => new(true, null);

    public static PasswordResetResendCodeResponse TooSoon(
        int resendAvailableInSeconds)
        => new(false, PasswordResetError.ResendTooSoon);

    public static PasswordResetResendCodeResponse Expired()
        => new(false, PasswordResetError.Expired);

    public static PasswordResetResendCodeResponse NotFound()
        => new(false, PasswordResetError.NotFound);

    public static PasswordResetResendCodeResponse TooManyAttempts()
        => new(false, PasswordResetError.TooManyAttempts);

    public static PasswordResetResendCodeResponse UserSuspended()
        => new(false, PasswordResetError.UserSuspended);
}
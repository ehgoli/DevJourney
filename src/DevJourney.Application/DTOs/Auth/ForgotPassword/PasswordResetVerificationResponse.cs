namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetVerificationResponse(
    bool Success,
    string? ResetToken,
    PasswordResetError? Error)
{
    public static PasswordResetVerificationResponse Verified(string resetToken)
        => new(true, resetToken, null);

    public static PasswordResetVerificationResponse InvalidCode()
        => new(false, null, PasswordResetError.InvalidCode);

    public static PasswordResetVerificationResponse Expired()
        => new(false, null, PasswordResetError.Expired);

    public static PasswordResetVerificationResponse TooManyAttempts()
        => new(false, null, PasswordResetError.TooManyAttempts);

    public static PasswordResetVerificationResponse NotFound()
        => new(false, null, PasswordResetError.NotFound);
}
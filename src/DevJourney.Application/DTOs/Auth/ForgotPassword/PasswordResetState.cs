namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetState(
    Guid UserId,
    string CodeHash,
    DateTimeOffset ExpiresAt,
    DateTimeOffset NextResendAt,
    int FailedAttempts);
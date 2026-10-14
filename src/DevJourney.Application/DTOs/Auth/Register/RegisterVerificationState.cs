namespace DevJourney.Application.DTOs.Auth.Register;

public sealed record RegisterVerificationState(
    string CodeHash,
    DateTimeOffset ExpiresAt,
    DateTimeOffset NextResendAt,
    int FailedAttempts);
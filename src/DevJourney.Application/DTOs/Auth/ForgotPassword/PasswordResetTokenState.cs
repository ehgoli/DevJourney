namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetTokenState(
    Guid UserId,
    DateTimeOffset ExpiresAt);
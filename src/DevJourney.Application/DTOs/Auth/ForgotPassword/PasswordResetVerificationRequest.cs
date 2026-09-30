namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetVerificationRequest(
    string PhoneNumber,
    string Code);
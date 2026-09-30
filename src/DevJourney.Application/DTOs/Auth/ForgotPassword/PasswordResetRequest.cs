namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetRequest(
    string PhoneNumber);
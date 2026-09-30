namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetResendCodeRequest(
    string PhoneNumber);
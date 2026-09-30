namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public record PasswordResetConfirmationRequest(string ResetToken, string NewPassword);
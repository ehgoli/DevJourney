namespace DevJourney.Application.DTOs.Auth.ForgotPassword;

public sealed record PasswordResetConfirmationResponse(
    bool IsSuccess,
    PasswordResetError? Error)
{
    public static PasswordResetConfirmationResponse Success()
        => new(true, null);
    
    public static PasswordResetConfirmationResponse NotFound()
        => new(false, PasswordResetError.NotFound);
    
    public static PasswordResetConfirmationResponse InvalidToken()
        => new(false, PasswordResetError.InvalidToken);

    public static PasswordResetConfirmationResponse Failed()
        => new(false, PasswordResetError.Unexpected);

}
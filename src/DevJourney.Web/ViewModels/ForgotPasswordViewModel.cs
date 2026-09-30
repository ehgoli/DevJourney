namespace DevJourney.Web.ViewModels;

public sealed class ForgotPasswordViewModel
{
    public int CurrentStep { get; set; } = 1;

    public string? ErrorMessage { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? ResetToken { get; set; }

    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}
namespace DevJourney.Web.ViewModels.Auth;

public sealed class RegisterViewModel
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool AcceptTerms { get; set; }

    public bool IsVerificationCodeSent { get; set; }

    public string? ErrorMessage { get; set; }
}
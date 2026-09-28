using System.ComponentModel.DataAnnotations;

namespace DevJourney.Web.ViewModels.Auth;

public class LoginViewModel
{
    public LoginViewModel()
    {
    }

    [DataType(DataType.PhoneNumber)]
    [Required]
    public string Phone { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; } = false;
    
    public string? ErrorMessage { get; set; }
}
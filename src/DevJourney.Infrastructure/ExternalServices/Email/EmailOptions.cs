using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.ExternalServices.Email;

public class EmailOptions
{
    public const string SectionName = "Email";
    
    [Required]
    [StringLength(
        253,
        MinimumLength = 1,
        ErrorMessage = "SMTP host must be between 1 and 253 characters.")]
    [RegularExpression(
        @"^\S+$",
        ErrorMessage = "SMTP host cannot contain whitespace.")]
    public string SmtpHost { get; set; } = string.Empty;

    [Range(
        1,
        65535,
        ErrorMessage = "SMTP port must be between 1 and 65535.")]
    public int SmtpPort { get; set; } = 587;

    public bool EnableSsl { get; set; } = true;

    [Required]
    [StringLength(
        200,
        MinimumLength = 1,
        ErrorMessage = "From name must be between 1 and 200 characters.")]
    [RegularExpression(
        @".*\S.*",
        ErrorMessage = "From name cannot be empty or whitespace.")]
    public string FromName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "From email must be a valid email address.")]
    [StringLength(
        320,
        MinimumLength = 3,
        ErrorMessage = "From email must be between 3 and 320 characters.")]
    public string FromEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(
        512,
        MinimumLength = 1,
        ErrorMessage = "Email password must be between 1 and 512 characters.")]
    [RegularExpression(
        @".*\S.*",
        ErrorMessage = "Email password cannot be empty or whitespace.")]
    public string Password { get; set; } = string.Empty;
}
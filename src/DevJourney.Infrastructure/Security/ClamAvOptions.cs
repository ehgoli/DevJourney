using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.Security;

public sealed class ClamAvOptions
{
    public const string SectionName = "ClamAV";
    
    [Required]
    [StringLength(
        253,
        MinimumLength = 1,
        ErrorMessage = "ClamAV host must be between 1 and 253 characters.")]
    [RegularExpression(
        @"^\S+$",
        ErrorMessage = "ClamAV host cannot contain whitespace.")]
    public string Host { get; set; } = "localhost";

    [Range(
        1,
        65535,
        ErrorMessage = "ClamAV port must be between 1 and 65535.")]
    public int Port { get; set; } = 3310;

    [Range(
        1,
        300,
        ErrorMessage = "ClamAV timeout must be between 1 and 300 seconds.")]
    public int TimeoutSeconds { get; set; } = 30;
}
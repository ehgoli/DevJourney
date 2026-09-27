using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.AI;

public sealed class GeminiOptions
{
    public const string SectionName = "Gemini";
    
    [Required]
    [RegularExpression(@".*\S.*", ErrorMessage = "Gemini API key cannot be empty or whitespace.")]
    [StringLength(512, ErrorMessage = "Gemini API key cannot exceed 512 characters.")]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\S+$", ErrorMessage = "Gemini model cannot contain whitespace.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Gemini model must be between 1 and 100 characters.")]
    public string Model { get; set; } = "gemini-3.8-flash";

    [Range(0.0, 2.0, ErrorMessage = "Gemini temperature must be between 0 and 2.")]
    public double Temperature { get; set; } = 0.1;

    [Range(1, 65536, ErrorMessage = "Gemini max output tokens must be between 1 and 65536.")]
    public int MaxOutputTokens { get; set; } = 256;
}
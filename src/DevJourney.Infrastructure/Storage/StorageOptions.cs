using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.Storage;

public sealed class StorageOptions
{
    public const string SectionName = "Storage";
    
    [Required]
    [StringLength(
        500,
        MinimumLength = 1,
        ErrorMessage = "Storage root path must be between 1 and 500 characters.")]
    [RegularExpression(
        @".*\S.*",
        ErrorMessage = "Storage root path cannot be empty or whitespace.")]
    public string RootPath { get; set; } = "wwwroot/uploads";
}
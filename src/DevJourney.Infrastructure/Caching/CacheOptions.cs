using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.Caching;

public sealed class CacheOptions
{
    public const string SectionName = "Caching";
    
    [Range(
        typeof(TimeSpan),
        "00:00:01",
        "7.00:00:00",
        ErrorMessage = "Default cache expiration must be between 1 second and 7 days.")]
    public TimeSpan DefaultExpiration { get; set; } =
        TimeSpan.FromMinutes(30);
}
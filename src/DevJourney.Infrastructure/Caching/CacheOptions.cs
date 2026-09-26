namespace DevJourney.Infrastructure.Caching;

public sealed class CacheOptions
{
    public const string SectionName = "Caching";
    
    public TimeSpan DefaultExpiration { get; set; } =
        TimeSpan.FromMinutes(30);
}
namespace DevJourney.Application.Interfaces.Infrastructure.Caching;

public static class CacheKeys
{
    public static string Project(
        Guid projectId,
        string language)
        => $"project:{projectId}:{language}";

    public static string Article(
        Guid articleId,
        string language)
        => $"article:{articleId}:{language}";

    public static string Homepage(
        string language)
        => $"homepage:{language}";
}
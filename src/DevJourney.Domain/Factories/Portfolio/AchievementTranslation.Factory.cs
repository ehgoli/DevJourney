namespace DevJourney.Domain.Entities.Portfolio;

public partial class AchievementTranslation
{
    private AchievementTranslation()
    {
    }
    
    private AchievementTranslation(Guid achievementId, string language, string title, string? description)
    {
        this.AchievementId = achievementId;
        this.Language = language;
        this.Title = title;
        this.Description = description;
        
        Validate(
            achievementId,
            language,
            title);
    }
    
    internal static AchievementTranslation Create(
        Guid achievementId,
        string language,
        string title,
        string? description)
    {
        return new AchievementTranslation(
            achievementId: achievementId,
            language: language,
            title: title,
            description: description);
    }
    
    public void Modify(
        string language,
        string title,
        string? description)
    {
        Validate(
            AchievementId,
            language,
            title);

        this.Language = language.Trim();
        this.Title = title.Trim();
        this.Description = description?.Trim();
    }

    
    private static void Validate(
        Guid achievementId,
        string language,
        string title)
    {
        if (achievementId == Guid.Empty)
            throw new InvalidOperationException(
                "Achievement ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(language))
            throw new InvalidOperationException(
                "Language cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException(
                "Achievement title cannot be empty.");
    }
}
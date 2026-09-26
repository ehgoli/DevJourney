using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Portfolio;

public partial class AchievementTranslation
{
    private AchievementTranslation()
    {
    }
    
    private AchievementTranslation(Guid achievementId, Language language, string title, string? description)
    {
        this.AchievementId = achievementId;
        this.Language = language;
        this.Title = title;
        this.Description = description;
        
        Validate(achievementId, title);
    }
    
    internal static AchievementTranslation Create(
        Guid achievementId,
        Language language,
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
        Language language,
        string title,
        string? description)
    {
        Validate(AchievementId, title);

        this.Language = language;
        this.Title = title.Trim();
        this.Description = description?.Trim();
    }

    
    private static void Validate(Guid achievementId, string title)
    {
        if (achievementId == Guid.Empty)
            throw new DomainException(
                "Achievement ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(
                "Achievement title cannot be empty.");
    }
}
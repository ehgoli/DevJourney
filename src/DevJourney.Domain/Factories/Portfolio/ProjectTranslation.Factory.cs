using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Portfolio;

public partial class ProjectTranslation
{
    private ProjectTranslation()
    {
    }

    private ProjectTranslation(
        Guid projectId,
        Language language,
        string title,
        string shortDescription,
        string description)
    {
        Validate(
            projectId,
            title,
            shortDescription,
            description);

        ProjectId = projectId;
        Language = language;
        Title = title.Trim();
        ShortDescription = shortDescription.Trim();
        Description = description.Trim();
    }

    internal static ProjectTranslation Create(
        Guid projectId,
        Language language,
        string title,
        string shortDescription,
        string description)
    {
        return new ProjectTranslation(
            projectId: projectId,
            language: language,
            title: title,
            shortDescription: shortDescription,
            description: description);
    }

    public void Modify(
        Language language,
        string title,
        string shortDescription,
        string description)
    {
        Validate(
            ProjectId,
            title,
            shortDescription,
            description);

        Language = language;
        Title = title.Trim();
        ShortDescription = shortDescription.Trim();
        Description = description.Trim();
    }

    private static void Validate(
        Guid projectId,
        string title,
        string shortDescription,
        string description)
    {
        if (projectId == Guid.Empty)
            throw new InvalidOperationException(
                "Project ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException(
                "Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(shortDescription))
            throw new InvalidOperationException(
                "Short description cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException(
                "Description cannot be empty.");
    }
}
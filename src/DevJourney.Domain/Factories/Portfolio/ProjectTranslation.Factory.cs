using DevJourney.Domain.Exceptions;
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
            throw new DomainException(
                "Project ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(
                "Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(shortDescription))
            throw new DomainException(
                "Short description cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(
                "Description cannot be empty.");
    }
}
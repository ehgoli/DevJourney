using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Articles;

public partial class CategoryTranslation
{
    private CategoryTranslation()
    {
    }

    private CategoryTranslation(
        Guid categoryId,
        Language language,
        string title,
        string description)
    {
        Validate(
            categoryId,
            title,
            description);

        CategoryId = categoryId;
        Language = language;
        Title = title;
        Description = description;
    }

    internal static CategoryTranslation Create(
        Guid categoryId,
        Language language,
        string title,
        string description)
    {
        return new CategoryTranslation(
            categoryId: categoryId,
            language: language,
            title: title,
            description: description);
    }

    public void Modify(string title, string description)
    {
        Validate(
            CategoryId,
            title,
            description);

        this.Title = title;
        this.Description = description;
    }

    private static void Validate(
        Guid categoryId,
        string title,
        string description)
    {
        if (categoryId == Guid.Empty)
            throw new DomainException(
                "Category ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(
                "Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(
                "Description cannot be empty.");
    }
}
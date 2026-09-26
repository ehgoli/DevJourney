namespace DevJourney.Domain.Entities.Articles;

public partial class CategoryTranslation
{
    private CategoryTranslation()
    {
    }

    private CategoryTranslation(
        Guid categoryId,
        string language,
        string title,
        string description)
    {
        Validate(
            categoryId,
            language,
            title,
            description);

        CategoryId = categoryId;
        Language = language;
        Title = title;
        Description = description;
    }

    internal static CategoryTranslation Create(
        Guid categoryId,
        string language,
        string title,
        string description)
    {
        return new CategoryTranslation(
            categoryId: categoryId,
            language: language,
            title: title,
            description: description);
    }

    public void Modify(
        string language,
        string title,
        string description)
    {
        Validate(
            CategoryId,
            language,
            title,
            description);

        Language = language;
        Title = title;
        Description = description;
    }

    private static void Validate(
        Guid categoryId,
        string language,
        string title,
        string description)
    {
        if (categoryId == Guid.Empty)
            throw new InvalidOperationException(
                "Category ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(language))
            throw new InvalidOperationException(
                "Language cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException(
                "Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException(
                "Description cannot be empty.");
    }
}
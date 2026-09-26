namespace DevJourney.Domain.Entities.Articles;

public partial class Category
{
    private Category()
    {
    }

    private Category(string? coverImage)
    {
        CoverImage = coverImage;
    }

    public static Category Create(string? coverImage)
    {
        return new Category(coverImage: coverImage);
    }

    public void Modify(string? coverImage)
    {
        this.CoverImage = coverImage;
    }

    public void AddTranslation(
        string language,
        string title,
        string description)
    {
        if (_translations.Any(x =>
            string.Equals(
                x.Language,
                language,
                StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"A translation for '{language}' already exists.");
        }

        var translation = CategoryTranslation.Create(
            categoryId: Id,
            language: language,
            title: title,
            description: description);

        _translations.Add(translation);
    }

    public void ModifyTranslation(
        Guid translationId,
        string language,
        string title,
        string description)
    {
        var translation = _translations.FirstOrDefault(x => x.Id == translationId)
            ?? throw new InvalidOperationException(
                $"Category translation '{translationId}' was not found.");

        if (_translations.Any(x =>
            x.Id != translationId &&
            string.Equals(
                x.Language,
                language,
                StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"A translation for '{language}' already exists.");
        }

        translation.Modify(
            language: language,
            title: title,
            description: description);
    }

    public void RemoveTranslation(Guid translationId)
    {
        var translation = _translations.FirstOrDefault(x => x.Id == translationId)
            ?? throw new InvalidOperationException(
                $"Category translation '{translationId}' was not found.");

        _translations.Remove(translation);
    }
}
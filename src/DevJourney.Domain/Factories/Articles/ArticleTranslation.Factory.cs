using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Articles;

public partial class ArticleTranslation
{
    private ArticleTranslation()
    {
    }

    private ArticleTranslation(Guid articleId, Language language, string title, string shortDescription, string content)
    {
        Validate(
            ArticleId,
            title,
            shortDescription,
            content);
        
        this.ArticleId = articleId;
        this.Language = language;
        this.Title = title;
        this.ShortDescription = shortDescription;
        this.Content = content;
    }

    internal static ArticleTranslation Create(Guid articleId, Language language, string title, string shortDescription, string content)
    {
        return new ArticleTranslation(
            articleId: articleId,
            language: language,
            title: title,
            shortDescription: shortDescription,
            content: content);
    }

    public void Modify(string title, string shortDescription, string content)
    {
        Validate(
            ArticleId,
            title,
            shortDescription,
            content);
        
        this.Title = title;
        this.ShortDescription = shortDescription;
        this.Content = content;
    }
    
    
    internal void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new InvalidOperationException(
                "Tag cannot be empty.");
        }

        tag = tag.Trim();

        if (_tags.Any(x =>
                string.Equals(
                    x,
                    tag,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"Tag '{tag}' already exists.");
        }

        _tags.Add(tag);
    }

    internal void RemoveTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new InvalidOperationException(
                "Tag cannot be empty.");
        }

        var existingTag = _tags.FirstOrDefault(
            x => string.Equals(
                x,
                tag.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (existingTag is null)
        {
            throw new InvalidOperationException(
                $"Tag '{tag}' was not found.");
        }

        _tags.Remove(existingTag);
    }
    
    private static void Validate(
        Guid articleId,
        string title,
        string shortDescription,
        string content)
    {
        if (articleId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Article ID cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException(
                "Article title cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(shortDescription))
        {
            throw new InvalidOperationException(
                "Article short description cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(
                "Article content cannot be empty.");
        }
    }
}
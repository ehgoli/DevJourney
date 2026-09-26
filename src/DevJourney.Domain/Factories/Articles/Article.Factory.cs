using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Articles;

public partial class Article
{
    private Article()
    {
    }

    private Article(string coverImage,
        DateOnly articleDate, 
        int readingTime, 
        Guid categoryId, 
        ArticleStatus status, 
        DateTime? scheduledAt)
    {
        this.CoverImage = coverImage;
        this.ArticleDate = articleDate;
        this.ReadingTime = readingTime;
        this.CategoryId = categoryId;
        this.Status = status;
        this.ScheduledAt = scheduledAt;
    }

    public static Article Create(string coverImage,
        DateOnly articleDate,
        int readingTime,
        Guid categoryId,
        ArticleStatus status,
        DateTime? scheduledAt)
    {
        Validate(
            articleDate,
            readingTime,
            categoryId);
        
        return new Article(
            coverImage: coverImage,
            articleDate: articleDate,
            readingTime: readingTime,
            categoryId: categoryId,
            status: status,
            scheduledAt: scheduledAt
        );
    }
    
    public void Modify(string coverImage,
        DateOnly articleDate, 
        int readingTime, 
        Guid categoryId, 
        ArticleStatus status, 
        DateTime? scheduledAt)
    {
        Validate(
            articleDate,
            readingTime,
            categoryId);
        
        this.CoverImage = coverImage;
        this.ArticleDate = articleDate;
        this.ReadingTime = readingTime;
        this.CategoryId = categoryId;
        this.Status = status;
        this.ScheduledAt = scheduledAt;
    }

    public void AddComment(Guid? userId,
        Guid? parentCommentId,
        string name,
        string email,
        Language language,
        string content)
    {
        var comment = Comment.Create(
            articleId: this.Id,
            userId: userId,
            parentCommentId: parentCommentId,
            name: name,
            email: email,
            language: language,
            content: content);
        
        this._comments.Add(comment);
    }

    public void AddTranslation(Guid articleId, Language language, string title, string shortDescription, string content)
    {
        if (_translations.Any(x => x.Language == language))
        {
            throw new DomainException(
                $"A translation for '{language}' already exists.");
        }

        var translation = ArticleTranslation.Create(
            articleId: articleId,
            language: language,
            title: title,
            shortDescription: shortDescription,
            content: content);
        
        this._translations.Add(translation);
    }
    
    public void MarkAsReadyToPublish()
    {
        if (Status != ArticleStatus.Draft)
        {
            throw new DomainException(
                "Only draft articles can be marked as ready to publish.");
        }

        if (_translations.Count == 0)
        {
            throw new DomainException(
                "Article must have at least one translation.");
        }

        Status = ArticleStatus.ReadyToPublish;
        ScheduledAt = null;
    }

    public void Schedule(DateTime scheduledAt)
    {
        if (Status != ArticleStatus.ReadyToPublish)
        {
            throw new DomainException(
                "Only articles ready to publish can be scheduled.");
        }

        if (scheduledAt <= DateTimeOffset.UtcNow)
        {
            throw new DomainException(
                "Scheduled time must be in the future.");
        }

        Status = ArticleStatus.Scheduled;
        ScheduledAt = scheduledAt;
    }

    public void CancelSchedule()
    {
        if (Status != ArticleStatus.Scheduled)
        {
            throw new DomainException(
                "Only scheduled articles can cancel their schedule.");
        }

        Status = ArticleStatus.ReadyToPublish;
        ScheduledAt = null;
    }

    public void Publish()
    {
        if (Status != ArticleStatus.ReadyToPublish &&
            Status != ArticleStatus.Scheduled)
        {
            throw new DomainException(
                "Article cannot be published from its current state.");
        }

        if (Status == ArticleStatus.Scheduled &&
            (!ScheduledAt.HasValue ||
             ScheduledAt.Value > DateTimeOffset.UtcNow))
        {
            throw new DomainException(
                "Scheduled article cannot be published before its scheduled time.");
        }

        Status = ArticleStatus.Published;
        ScheduledAt = null;
    }
    
    private static void Validate(
        DateOnly articleDate,
        int readingTime,
        Guid categoryId)
    {
        if (readingTime <= 0)
        {
            throw new DomainException(
                "Reading time must be greater than zero.");
        }

        if (categoryId == Guid.Empty)
        {
            throw new DomainException(
                "Category ID cannot be empty.");
        }
    }
}
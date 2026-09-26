using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Articles;

public partial class Comment
{
    private Comment()
    {
    }

    private Comment(
        Guid articleId,
        Guid? userId,
        Guid? parentCommentId,
        string name,
        string email,
        string content,
        Language language,
        CommentStatus status)
    {
        Validate(
            articleId,
            userId,
            parentCommentId,
            name,
            email,
            content);

        this.ArticleId = articleId;
        this.UserId = userId;
        this.ParentCommentId = parentCommentId;
        this.Name = name.Trim();
        this. Email = email.Trim();
        this.Content = content.Trim();
        this.Language = language;
        this.Status = status;
    }

    internal static Comment Create(
        Guid articleId,
        Guid? userId,
        Guid? parentCommentId,
        string name,
        string email,
        Language language,
        string content)
    {
        return new Comment(
            articleId: articleId,
            userId: userId,
            parentCommentId: parentCommentId,
            name: name,
            email: email,
            content: content,
            language: language,
            status: CommentStatus.Pending);
    }

    public void Modify(
        string name,
        string email,
        string content)
    {
        Validate(
            ArticleId,
            UserId,
            ParentCommentId,
            name,
            email,
            content);

        this.Name = name.Trim();
        this.Email = email.Trim();
        this.Content = content.Trim();
    }

    public void Approve()
    {
        if (Status == CommentStatus.Approved)
            return;

        Status = CommentStatus.Approved;
    }

    public void Flag()
    {
        if (Status == CommentStatus.Flagged)
            return;

        Status = CommentStatus.Flagged;
    }

    public void Reject()
    {
        if (Status == CommentStatus.Rejected)
            return;

        Status = CommentStatus.Rejected;
    }
    
    
    
    private static void Validate(
        Guid articleId,
        Guid? userId,
        Guid? parentCommentId,
        string name,
        string email,
        string content)
    {
        if (articleId == Guid.Empty)
            throw new InvalidOperationException(
                "Article ID cannot be empty.");

        if (userId.HasValue && userId.Value == Guid.Empty)
            throw new InvalidOperationException(
                "User ID cannot be empty.");

        if (parentCommentId.HasValue &&
            parentCommentId.Value == Guid.Empty)
            throw new InvalidOperationException(
                "Parent comment ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException(
                "Comment name cannot be empty.");

        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException(
                "Comment content cannot be empty.");
        
        ValidateEmail(email);
    }
    
    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException(
                "Comment email cannot be empty.");

        try
        {
            var address = new System.Net.Mail.MailAddress(email);

            if (address.Address != email.Trim())
                throw new InvalidOperationException(
                    "Comment email is invalid.");
        }
        catch (FormatException)
        {
            throw new InvalidOperationException(
                "Comment email is invalid.");
        }
    }
}
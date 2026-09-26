using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;


namespace DevJourney.Domain.Entities.Articles;

public sealed partial class Comment : BaseEntity
{
    public Guid ArticleId { get; private set; }

    public Guid? UserId { get; private set; }

    public Guid? ParentCommentId { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Content { get; private set; } = null!;

    public string Language { get; set; } = null!;
    
    public CommentStatus Status { get; private set; } = CommentStatus.Pending;
}
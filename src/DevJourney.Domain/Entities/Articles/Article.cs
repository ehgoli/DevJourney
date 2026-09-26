using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;


namespace DevJourney.Domain.Entities.Articles;

public sealed partial class Article : BaseEntity
{
    public string CoverImage { get; private set; } = null!;

    public DateOnly ArticleDate { get; private set; }

    public int ReadingTime { get; private set; }

    public Guid CategoryId { get; private set; }

    public ArticleStatus Status { get; private set; }

    public DateTime? ScheduledAt { get; private set; }

    public IReadOnlyCollection<ArticleTranslation> Translations => _translations;
    private readonly List<ArticleTranslation> _translations = [];
    
    public IReadOnlyCollection<Comment> Comments => _comments;
    private readonly List<Comment> _comments = [];
} 
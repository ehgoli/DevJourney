using System;
using System.Collections.Generic;
using System.Text;


using DevJourney.Domain.Entities.Common;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Articles;

public sealed partial class ArticleTranslation : BaseEntity
{
    public Guid ArticleId { get; private set; }

    public Language Language { get; private set; }

    public string Title { get; private set; } = null!;

    public string ShortDescription { get; private set; } = null!;

    public string Content { get; private set; } = null!;

    public IReadOnlyCollection<string> Tags => _tags;
    private readonly List<string> _tags = [];
}
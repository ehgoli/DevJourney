using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;
using DevJourney.Domain.ValueObjects;


namespace DevJourney.Domain.Entities.Articles;

public sealed partial class CategoryTranslation : BaseEntity
{
    public Guid CategoryId { get; private set; }

    public Language Language { get; private set; }

    public string Title { get; private set; } = null!;

    public string Description { get; private set; } = null!;
}
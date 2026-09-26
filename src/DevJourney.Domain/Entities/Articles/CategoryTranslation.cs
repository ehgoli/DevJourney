using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;


namespace DevJourney.Domain.Entities.Articles;

public sealed partial class CategoryTranslation : BaseEntity
{
    public Guid CategoryId { get; private set; }

    public string Language { get; private set; } = null!;

    public string Title { get; private set; } = null!;

    public string Description { get; private set; } = null!;
}
using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Portfolio;

public sealed partial class ProjectTranslation : BaseEntity
{
    public Guid ProjectId { get; private set; }

    public Language Language { get; private set; }

    public string Title { get; private set; } = null!;

    public string ShortDescription { get; private set; } = null!;

    public string Description { get; private set; } = null!;
}
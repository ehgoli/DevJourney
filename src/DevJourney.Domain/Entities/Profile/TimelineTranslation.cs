using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.ValueObjects;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class TimelineTranslation : BaseEntity
{
    public Guid TimelineId { get; private set; }

    public Language Language { get; private set; }

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
}
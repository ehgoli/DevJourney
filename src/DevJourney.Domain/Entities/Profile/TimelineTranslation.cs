using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class TimelineTranslation : BaseEntity
{
    public Guid TimelineId { get; private set; }

    public string Language { get; private set; } = null!;

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
}
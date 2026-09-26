using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class Timeline : BaseEntity
{
    public DateOnly FromDate { get; private set; }
    public DateOnly? ToDate { get; private set; }


    private readonly List<TimelineTranslation> _translations = [];

    public IReadOnlyCollection<TimelineTranslation> Translations => _translations;
}
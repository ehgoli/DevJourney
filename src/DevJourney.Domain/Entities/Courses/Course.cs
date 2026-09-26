using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Courses;

public sealed partial class Course : BaseEntity
{

    public string Title { get; private set; } = null!;

    public string Cover { get; private set; } = null!;

    public string ShortDescription { get; private set; } = null!;

    public CourseLevel Level { get; private set; }

    public TimeSpan Duration { get; private set; }

    public Language Language { get; private set; }

    public string Description { get; private set; } = null!;

    public string? Prerequisites { get; private set; }

    public CourseProductionStatus ProductionStatus { get; private set; }

    public CoursePublicationStatus PublicationStatus { get; private set; }

    public DateTimeOffset? ScheduledAt { get; private set; }

    public IReadOnlyCollection<Episode> Episodes => _episodes;
    
}
using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Courses;

public sealed partial class Episode : BaseEntity
{
    public Guid CourseId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Caption { get; private set; } = null!;

    public string? AttachmentFileName { get; private set; }

    public string VideoFileName { get; private set; } = null!;

    public bool IsFree { get; private set; }

    public TimeSpan VideoDuration { get; private set; }

    public DateTime PublishedAt { get; private set; } = DateTime.UtcNow;
}
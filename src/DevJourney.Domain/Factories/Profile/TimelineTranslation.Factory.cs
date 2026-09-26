using System;
using System.Collections.Generic;
using System.Text;


using DevJourney.Domain.Entities.Common;


namespace DevJourney.Domain.Entities.Profile;

public partial class TimelineTranslation
{
    private TimelineTranslation()
    {
    }

    private TimelineTranslation(
        Guid timelineId,
        string language,
        string title,
        string description)
    {
        Validate(timelineId, language, title, description);

        this.TimelineId = timelineId;
        this.Language = language;
        this.Title = title;
        this.Description = description;
    }

    internal static TimelineTranslation Create(
        Guid timelineId,
        string language,
        string title,
        string description)
    {
        return new(
            timelineId: timelineId,
            language: language,
            title: title,
            description: description);
    }

    public void Modify(
        Guid timelineId,
        string language,
        string title,
        string description)
    {
        Validate(timelineId, language, title, description);

        this.TimelineId = timelineId;
        this.Language = language;
        this.Title = title;
        this.Description = description;
    }

    private static void Validate(
        Guid timelineId,
        string language,
        string title,
        string description)
    {
        if (timelineId == Guid.Empty)
            throw new InvalidOperationException(
                "Timeline ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(language))
            throw new InvalidOperationException(
                "Language cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException(
                "Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException(
                "Description cannot be empty.");
    }
}
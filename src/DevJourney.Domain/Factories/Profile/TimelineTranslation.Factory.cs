using System;
using System.Collections.Generic;
using System.Text;


using DevJourney.Domain.Entities.Common;
using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;


namespace DevJourney.Domain.Entities.Profile;

public partial class TimelineTranslation
{
    private TimelineTranslation()
    {
    }

    private TimelineTranslation(
        Guid timelineId,
        Language language,
        string title,
        string description)
    {
        Validate(timelineId, title, description);

        this.TimelineId = timelineId;
        this.Language = language;
        this.Title = title;
        this.Description = description;
    }

    internal static TimelineTranslation Create(
        Guid timelineId,
        Language language,
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
        Language language,
        string title,
        string description)
    {
        Validate(timelineId, title, description);

        this.TimelineId = timelineId;
        this.Language = language;
        this.Title = title;
        this.Description = description;
    }

    private static void Validate(
        Guid timelineId,
        string title,
        string description)
    {
        if (timelineId == Guid.Empty)
            throw new DomainException(
                "Timeline ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(
                "Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(
                "Description cannot be empty.");
    }
}
using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Courses;

public partial class Course
{
    #region Constructor

    private Course()
    {
    }
    
    private Course(
        string title,
        string cover,
        string shortDescription,
        CourseLevel level,
        Language language,
        string description,
        string? prerequisites)
    {
        Validate(
            title: title,
            cover: cover,
            shortDescription: shortDescription,
            description: description);

        Title = title.Trim();
        Cover = cover.Trim();
        ShortDescription = shortDescription.Trim();
        Level = level;
        Language = language;
        Description = description.Trim();
        Prerequisites = prerequisites?.Trim();

        ProductionStatus = CourseProductionStatus.Recording;
        PublicationStatus = CoursePublicationStatus.Draft;
        ScheduledAt = null;
        Duration = TimeSpan.Zero;
    }

    #endregion

    #region Factory & Modification

    public static Course Create(
        string title,
        string cover,
        string shortDescription,
        CourseLevel level,
        Language language,
        string description,
        string? prerequisites = null)
    {
        return new Course(
            title: title,
            cover: cover,
            shortDescription: shortDescription,
            level: level,
            language: language,
            description: description,
            prerequisites: prerequisites);
    }

    public void Modify(
        string title,
        string cover,
        string shortDescription,
        CourseLevel level,
        string description,
        string? prerequisites = null)
    {
        Validate(
            title: title,
            cover: cover,
            shortDescription: shortDescription,
            description: description);

        Title = title.Trim();
        Cover = cover.Trim();
        ShortDescription = shortDescription.Trim();
        Level = level;
        Description = description.Trim();
        Prerequisites = prerequisites?.Trim();
    }

    #endregion
    
    #region Publication & Production

        public void StartRecording()
    {
        if (ProductionStatus == CourseProductionStatus.Discontinued)
            throw new DomainException(
                "A discontinued course cannot be started.");

        if (ProductionStatus == CourseProductionStatus.Completed)
            throw new DomainException(
                "A completed course cannot be started again.");

        ProductionStatus = CourseProductionStatus.Recording;
    }

    public void CompleteRecording()
    {
        if (ProductionStatus != CourseProductionStatus.Recording)
            throw new DomainException(
                "Only a course in recording state can be completed.");

        if (_episodes.Count == 0)
            throw new DomainException(
                "A course must have at least one episode before it can be completed.");

        ProductionStatus = CourseProductionStatus.Completed;
    }

    public void Discontinue()
    {
        if (ProductionStatus == CourseProductionStatus.Discontinued)
            return;

        ProductionStatus = CourseProductionStatus.Discontinued;

        PublicationStatus = CoursePublicationStatus.Draft;
        ScheduledAt = null;
    }

    public void Activate()
    {
        if (ProductionStatus != CourseProductionStatus.Completed)
            throw new DomainException(
                "Only a completed course can be activated.");

        PublicationStatus = CoursePublicationStatus.Active;
        ScheduledAt = null;
    }

    public void Deactivate()
    {
        if (PublicationStatus != CoursePublicationStatus.Active)
            throw new DomainException(
                "Only an active course can be deactivated.");

        PublicationStatus = CoursePublicationStatus.Draft;
        ScheduledAt = null;
    }

    public void Schedule(DateTimeOffset scheduledAt)
    {
        if (ProductionStatus != CourseProductionStatus.Completed)
            throw new DomainException(
                "Only a completed course can be scheduled.");

        if (PublicationStatus == CoursePublicationStatus.Active)
            throw new DomainException(
                "An active course cannot be scheduled.");

        if (scheduledAt <= DateTimeOffset.UtcNow)
            throw new DomainException(
                "Scheduled time must be in the future.");

        PublicationStatus = CoursePublicationStatus.Scheduled;
        ScheduledAt = scheduledAt;
    }

    public void CancelSchedule()
    {
        if (PublicationStatus != CoursePublicationStatus.Scheduled)
            throw new DomainException(
                "Only a scheduled course can cancel its schedule.");

        PublicationStatus = CoursePublicationStatus.Draft;
        ScheduledAt = null;
    }

    #endregion

    #region Episodes

    public void AddEpisode(
        string title,
        string caption,
        string? attachmentFileName,
        string videoFileName,
        bool isFree,
        TimeSpan videoDuration)
    {
        var episode = Episode.Create(
            courseId: this.Id,
            title: title,
            caption: caption,
            attachmentFileName: attachmentFileName,
            videoFileName: videoFileName,
            isFree: isFree,
            videoDuration: videoDuration,
            publishedAt: DateTime.UtcNow);
    
        _episodes.Add(episode);
    }

    #endregion

    #region Validation

    private static void Validate(
        string title,
        string cover,
        string shortDescription,
        string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(
                "Course title cannot be empty.");

        if (string.IsNullOrWhiteSpace(cover))
            throw new DomainException(
                "Course cover cannot be empty.");

        if (string.IsNullOrWhiteSpace(shortDescription))
            throw new DomainException(
                "Course short description cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(
                "Course description cannot be empty.");
    }

    #endregion

    #region Calculated Properties

    private readonly List<Episode> _episodes = [];

    public int EpisodeCount => _episodes.Count;
    
    public IReadOnlyCollection<string> GetPrerequisites()
    {
        if (string.IsNullOrWhiteSpace(Prerequisites))
            return [];

        return Prerequisites
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();
    }
    
    private static string? BuildPrerequisites(
        IEnumerable<string>? prerequisites)
    {
        if (prerequisites is null)
            return null;

        var items = prerequisites
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return items.Length == 0
            ? null
            : string.Join(", ", items);
    }

    #endregion
}
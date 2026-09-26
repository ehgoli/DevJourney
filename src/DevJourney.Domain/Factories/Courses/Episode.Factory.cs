namespace DevJourney.Domain.Entities.Courses;

public partial class Episode
{
    private Episode()
    {
    }

    public Episode(Guid courseId,
        string title,
        string caption,
        string? attachmentFileName,
        string videoFileName,
        bool isFree,
        TimeSpan videoDuration,
        DateTime publishedAt)
    {
        this.CourseId = courseId;
        this.Title = title;
        this.Caption = caption;
        this.AttachmentFileName = attachmentFileName;
        this.VideoFileName = videoFileName;
        this.IsFree = isFree;
        this.VideoDuration = videoDuration;
        this.PublishedAt = publishedAt;
    }

    public static Episode Create(Guid courseId,
        string title,
        string caption,
        string? attachmentFileName,
        string videoFileName,
        bool isFree,
        TimeSpan videoDuration,
        DateTime publishedAt)
    {
        return new Episode(
            courseId: courseId,
            title: title,
            caption: caption,
            attachmentFileName: attachmentFileName,
            videoFileName: videoFileName,
            isFree: isFree,
            videoDuration: videoDuration,
            publishedAt: publishedAt
        );
    }
}
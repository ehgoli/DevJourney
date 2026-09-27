namespace DevJourney.Application.Interfaces.Infrastructure.Media;

public sealed record ImageProcessingResult(
    bool IsImage,
    Stream Content,
    string FileName,
    string ContentType,
    long Length);
namespace DevJourney.Application.Interfaces.Infrastructure.Media;

public interface IImageProcessor
{
    Task<ImageProcessingResult> ProcessAsync(
        Stream content,
        string fileName,
        CancellationToken cancellationToken = default);
}
namespace DevJourney.Application.Interfaces.Infrastructure.Storage;

public interface IFileStorage
{
    Task<StoredFile> SaveAsync(
        Stream content,
        string fileName,
        string folder,
        CancellationToken cancellationToken = default);

    Task<Stream?> OpenReadAsync(
        string fileName,
        string folder,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string fileName,
        string folder,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string fileName,
        string folder,
        CancellationToken cancellationToken = default);
}
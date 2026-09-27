namespace DevJourney.Application.Interfaces.Infrastructure.Security;

public interface IFileScanner
{
    Task<FileScanResult> ScanAsync(
        Stream content,
        CancellationToken cancellationToken = default);
}
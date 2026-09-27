using DevJourney.Application.Interfaces.Infrastructure.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DevJourney.Infrastructure.Storage;

public sealed class LocalFileStorage(
    IHostEnvironment environment,
    IOptions<StorageOptions> options) : IFileStorage
{
    private readonly string _rootPath = GetRootPath(
        environment,
        options.Value);

    public async Task<StoredFile> SaveAsync(
        Stream content,
        string fileName,
        string folder,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        cancellationToken.ThrowIfCancellationRequested();

        var safeExtension = Path.GetExtension(
            Path.GetFileName(fileName));

        var storedFileName =
            Path.ChangeExtension(
                Path.GetRandomFileName(),
                safeExtension);

        var directoryPath = GetDirectoryPath(folder);

        Directory.CreateDirectory(directoryPath);

        var filePath = Path.Combine(
            directoryPath,
            storedFileName);

        await using var fileStream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 64 * 1024,
            options: FileOptions.Asynchronous | FileOptions.SequentialScan);

        await content.CopyToAsync(
            fileStream,
            cancellationToken);

        var contentType = GetContentType(
            safeExtension);

        return new StoredFile(
            FileName: storedFileName,
            ContentType: contentType,
            Length: fileStream.Length);
    }

    public Task<Stream?> OpenReadAsync(
        string fileName,
        string folder,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        cancellationToken.ThrowIfCancellationRequested();

        var filePath = GetFilePath(
            fileName,
            folder);

        if (!File.Exists(filePath))
            return Task.FromResult<Stream?>(null);

        Stream stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 64 * 1024,
            options: FileOptions.Asynchronous | FileOptions.SequentialScan);

        return Task.FromResult<Stream?>(stream);
    }

    public Task<bool> ExistsAsync(
        string fileName,
        string folder,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        cancellationToken.ThrowIfCancellationRequested();

        var filePath = GetFilePath(
            fileName,
            folder);

        return Task.FromResult(
            File.Exists(filePath));
    }

    public Task DeleteAsync(
        string fileName,
        string folder,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        cancellationToken.ThrowIfCancellationRequested();

        var filePath = GetFilePath(
            fileName,
            folder);

        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }

    private string GetDirectoryPath(string folder)
    {
        ValidateFolder(folder);

        var directoryPath = Path.GetFullPath(
            Path.Combine(
                _rootPath,
                folder));

        EnsurePathInsideRoot(directoryPath);

        return directoryPath;
    }

    private string GetFilePath(
        string fileName,
        string folder)
    {
        var safeFileName = Path.GetFileName(fileName);

        if (string.IsNullOrWhiteSpace(safeFileName) ||
            !string.Equals(
                safeFileName,
                fileName,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Invalid file name.");
        }

        var directoryPath = GetDirectoryPath(folder);

        var filePath = Path.GetFullPath(
            Path.Combine(
                directoryPath,
                safeFileName));

        EnsurePathInsideRoot(filePath);

        return filePath;
    }

    private static void ValidateFolder(string folder)
    {
        if (Path.IsPathRooted(folder))
            throw new InvalidOperationException(
                "Storage folder must be relative.");

        var normalizedFolder = folder.Trim();

        if (normalizedFolder.Contains("..", StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Storage folder cannot contain parent directory traversal.");

        if (string.IsNullOrWhiteSpace(normalizedFolder))
            throw new InvalidOperationException(
                "Storage folder cannot be empty.");
    }

    private void EnsurePathInsideRoot(string path)
    {
        var root = Path.GetFullPath(_rootPath);

        if (!root.EndsWith(
                Path.DirectorySeparatorChar))
        {
            root += Path.DirectorySeparatorChar;
        }

        if (!path.StartsWith(
                root,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "The file path is outside the storage root.");
        }
    }

    private static string GetRootPath(
        IHostEnvironment environment,
        StorageOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.RootPath))
            throw new InvalidOperationException(
                "Storage root path cannot be empty.");

        return Path.GetFullPath(
            Path.IsPathRooted(options.RootPath)
                ? options.RootPath
                : Path.Combine(
                    environment.ContentRootPath,
                    options.RootPath));
    }

    private static string GetContentType(
        string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".svg" => "image/svg+xml",

            ".pdf" => "application/pdf",

            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".mov" => "video/quicktime",

            ".zip" => "application/zip",

            ".doc" => "application/msword",
            ".docx" =>
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",

            ".xls" => "application/vnd.ms-excel",
            ".xlsx" =>
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

            ".txt" => "text/plain",

            _ => "application/octet-stream"
        };
    }
}
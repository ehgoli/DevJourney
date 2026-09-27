namespace DevJourney.Application.Interfaces.Infrastructure.Storage;

public sealed record StoredFile(
    string FileName,
    string ContentType,
    long Length);
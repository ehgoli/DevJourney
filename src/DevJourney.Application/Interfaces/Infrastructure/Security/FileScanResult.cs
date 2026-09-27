namespace DevJourney.Application.Interfaces.Infrastructure.Security;

public sealed record FileScanResult(
    bool IsSafe,
    string? ThreatName = null);
namespace DevJourney.Application.Interfaces.Infrastructure.AI;

public interface IAiModerationService
{
    Task<AiModerationResult> ModerateAsync(
        string content,
        CancellationToken cancellationToken = default);
}
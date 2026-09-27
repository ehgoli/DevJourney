namespace DevJourney.Application.Interfaces.Infrastructure.AI;

public sealed record AiModerationResult(
    AiModerationDecision Decision,
    string? Reason,
    double? Confidence);
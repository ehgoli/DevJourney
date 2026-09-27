namespace DevJourney.Infrastructure.AI;

public sealed class GeminiOptions
{
    public string ApiKey { get; set; } = null!;

    public string Model { get; set; } = "gemini-3.8-flash";

    public double Temperature { get; set; } = 0.1;

    public int MaxOutputTokens { get; set; } = 256;
}
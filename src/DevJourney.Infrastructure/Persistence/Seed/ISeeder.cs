namespace DevJourney.Infrastructure.Persistence.Seed;

public interface ISeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
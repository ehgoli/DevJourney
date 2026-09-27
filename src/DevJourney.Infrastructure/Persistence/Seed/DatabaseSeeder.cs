using DevJourney.Infrastructure.Persistence.Seed.Articles;
using DevJourney.Infrastructure.Persistence.Seed.Courses;
using DevJourney.Infrastructure.Persistence.Seed.Identity;
using DevJourney.Infrastructure.Persistence.Seed.Portfolio;
using DevJourney.Infrastructure.Persistence.Seed.Profile;

namespace DevJourney.Infrastructure.Persistence.Seed;

public sealed class DatabaseSeeder(
    IEnumerable<ISeeder> seeders)
{
    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(cancellationToken);
        }
    }
}
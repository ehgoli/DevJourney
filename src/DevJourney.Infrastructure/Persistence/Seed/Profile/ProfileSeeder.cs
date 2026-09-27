using DevJourney.Domain.ValueObjects;
using DevJourney.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevJourney.Infrastructure.Persistence.Seed.Profile;

public sealed class ProfileSeeder(
    AppDbContext context) : ISeeder
{
    private const string SampleEmail = "email@service.com";
    private const string SamplePhone = "+15550000002";

    public int Order => 300;

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Set<Domain.Entities.Profile.Profile>()
            .AnyAsync(cancellationToken);

        if (exists)
            return;

        var profile =
            Domain.Entities.Profile.Profile.Create(
                email: SampleEmail,
                phone: SamplePhone);

        profile.AddTranslation(
            Language.Create("en"),
            "Ehsan Goli",
            ".NET Backend Developer",
            "Building modern web applications with C# and ASP.NET Core.",
            "This is a sample profile for the initial development and testing of DevJourney. "
            + "The content can be replaced later with the actual profile information.");

        await context.Set<Domain.Entities.Profile.Profile>()
            .AddAsync(profile, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
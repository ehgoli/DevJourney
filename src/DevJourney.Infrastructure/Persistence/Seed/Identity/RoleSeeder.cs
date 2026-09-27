using DevJourney.Domain.Entities.Identity;
using DevJourney.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevJourney.Infrastructure.Persistence.Seed.Identity;

public sealed class RoleSeeder(
    AppDbContext context) : ISeeder
{
    public int Order => 100;

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        var roles = new[]
        {
            (
                Name: "Admin",
                DisplayName: "Administrator",
                Description: "System administrator role."
            ),
            (
                Name: "User",
                DisplayName: "User",
                Description: "Standard application user role."
            )
        };

        var hasChanges = false;

        foreach (var roleData in roles)
        {
            var exists = await context.Set<Role>()
                .AnyAsync(
                    x => x.Name == roleData.Name,
                    cancellationToken);

            if (exists)
                continue;

            var role = Role.Create(
                roleData.Name,
                roleData.DisplayName,
                roleData.Description);

            await context.Set<Role>()
                .AddAsync(role, cancellationToken);

            hasChanges = true;
        }

        if (hasChanges)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
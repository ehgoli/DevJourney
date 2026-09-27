using DevJourney.Domain.Entities.Identity;
using DevJourney.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevJourney.Infrastructure.Persistence.Seed.Identity;

public sealed class AdminUserSeeder(
    AppDbContext context,
    IPasswordHasher<User> passwordHasher) : ISeeder
{
    private const string AdminRoleName = "Admin";

    private const string SampleFullName = "Admin User";
    private const string SampleEmail = "admin@example.com";
    private const string SamplePhone = "+15550000001";
    private const string SamplePassword = "DevJourney@1234";

    public int Order => 200;

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        var adminRole = await context.Set<Role>()
            .SingleOrDefaultAsync(
                x => x.Name == AdminRoleName,
                cancellationToken);

        if (adminRole is null)
        {
            throw new InvalidOperationException(
                $"Required role '{AdminRoleName}' was not found.");
        }

        var user = await context.Set<User>()
            .SingleOrDefaultAsync(
                x => x.Email == SampleEmail,
                cancellationToken);

        if (user is null)
        {
            var passwordHash = passwordHasher.HashPassword(
                null!,
                SamplePassword);

            user = User.Create(
                fullName: SampleFullName,
                email: SampleEmail,
                phone: SamplePhone,
                passwordHash: passwordHash,
                profileImage: null,
                activationCode: null);

            user.Activate();

            await context.Set<User>()
                .AddAsync(user, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        var hasAdminRole = await context.Set<UserRole>()
            .AnyAsync(
                x =>
                    x.UserId == user.Id &&
                    x.RoleId == adminRole.Id,
                cancellationToken);

        if (!hasAdminRole)
        {
            user.AddRole(adminRole.Id);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
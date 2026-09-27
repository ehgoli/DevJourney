using DevJourney.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Identity;

public sealed class RoleConfig
    : BaseEntityTypeConfiguration<Role>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .IsRequired(false);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
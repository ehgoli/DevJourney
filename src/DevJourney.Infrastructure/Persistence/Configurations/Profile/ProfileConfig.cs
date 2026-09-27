using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Profile;

public sealed class ProfileConfig
    : BaseEntityTypeConfiguration<Domain.Entities.Profile.Profile>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Domain.Entities.Profile.Profile> builder)
    {
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20);
    }
}
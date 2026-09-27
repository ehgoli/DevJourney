using DevJourney.Domain.Entities.Profile;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Profile;

public sealed class SocialMediaConfig
    : BaseEntityTypeConfiguration<SocialMedia>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<SocialMedia> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(x => x.Icon)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DisplayOrder)
            .IsRequired();
    }
}
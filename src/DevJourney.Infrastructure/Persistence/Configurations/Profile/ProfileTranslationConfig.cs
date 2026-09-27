using DevJourney.Domain.Entities.Profile;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DevJourney.Infrastructure.Persistence.Configurations.Profile;

public sealed class ProfileTranslationConfig
    : BaseEntityTypeConfiguration<ProfileTranslation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ProfileTranslation> builder)
    {
        builder.Property(x => x.ProfileId)
            .IsRequired();

        builder.Property(x => x.Language)
            .IsRequired()
            .HasConversion(
                language => language.Code,
                code => Language.Create(code))
            .HasMaxLength(2)
            .IsUnicode(false);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Heading)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Bio)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.About)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.ProfileId,
                x.Language
            })
            .IsUnique();
    }
}
using DevJourney.Domain.Entities.Profile;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Profile;

public sealed class TimelineTranslationConfig
    : BaseEntityTypeConfiguration<TimelineTranslation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<TimelineTranslation> builder)
    {
        builder.Property(x => x.TimelineId)
            .IsRequired();

        builder.Property(x => x.Language)
            .IsRequired()
            .HasConversion(
                language => language.Code,
                code => Language.Create(code))
            .HasMaxLength(2)
            .IsUnicode(false);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.TimelineId,
                x.Language
            })
            .IsUnique();
    }
}
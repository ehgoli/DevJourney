using DevJourney.Domain.Entities.Portfolio;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Portfolio;


public sealed class AchievementTranslationConfig
    : BaseEntityTypeConfiguration<AchievementTranslation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<AchievementTranslation> builder)
    {
        builder.Property(x => x.AchievementId)
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
            .IsRequired(false);

        builder.HasIndex(x => new
            {
                x.AchievementId,
                x.Language
            })
            .IsUnique();
    }
}
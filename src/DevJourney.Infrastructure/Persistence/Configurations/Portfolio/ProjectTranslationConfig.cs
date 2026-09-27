using DevJourney.Domain.Entities.Portfolio;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Portfolio;


public sealed class ProjectTranslationConfig
    : BaseEntityTypeConfiguration<ProjectTranslation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ProjectTranslation> builder)
    {
        builder.Property(x => x.ProjectId)
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

        builder.Property(x => x.ShortDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.ProjectId,
                x.Language
            })
            .IsUnique();
    }
}
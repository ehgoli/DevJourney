using DevJourney.Domain.Entities.Articles;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Articles;

public sealed class CategoryTranslationConfig
    : BaseEntityTypeConfiguration<CategoryTranslation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.Property(x => x.CategoryId)
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
                x.CategoryId,
                x.Language
            })
            .IsUnique();
    }
}
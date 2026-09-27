using DevJourney.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Articles;

public sealed class CategoryConfig
    : BaseEntityTypeConfiguration<Category>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.CoverImage)
            .HasMaxLength(255);

        builder.HasMany(x => x.Translations)
            .WithOne()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
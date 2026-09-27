using DevJourney.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Articles;

public sealed class ArticleConfig
    : BaseEntityTypeConfiguration<Article>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Article> builder)
    {
        builder.Property(x => x.CoverImage)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ArticleDate)
            .IsRequired();

        builder.Property(x => x.ReadingTime)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.ScheduledAt)
            .IsRequired(false);

        builder.HasIndex(x => x.CategoryId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.ArticleDate);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Translations)
            .WithOne()
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne()
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
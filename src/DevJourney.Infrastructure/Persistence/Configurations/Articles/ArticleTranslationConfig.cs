using System.Text.Json;
using DevJourney.Domain.Entities.Articles;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Articles;

public sealed class ArticleTranslationConfig
    : BaseEntityTypeConfiguration<ArticleTranslation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ArticleTranslation> builder)
    {
        builder.Property(x => x.ArticleId)
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

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property<List<string>>("_tags")
            .HasColumnName("Tags")
            .HasColumnType("nvarchar(max)")
            .HasConversion(
                tags => JsonSerializer.Serialize(
                    tags,
                    (JsonSerializerOptions?)null),

                json => JsonSerializer.Deserialize<List<string>>(
                    json,
                    (JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(
                new ValueComparer<List<string>>(
                    (left, right) =>
                        left != null &&
                        right != null &&
                        left.SequenceEqual(right),

                    value => value == null
                        ? 0
                        : value.Aggregate(
                            0,
                            (hash, item) =>
                                HashCode.Combine(hash, item.GetHashCode())),

                    value => value == null
                        ? new List<string>()
                        : value.ToList()));

        builder.HasIndex(x => new
        {
            x.ArticleId,
            x.Language
        })
        .IsUnique();
    }
}
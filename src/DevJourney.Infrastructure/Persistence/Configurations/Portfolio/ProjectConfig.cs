using DevJourney.Domain.Entities.Portfolio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Portfolio;

public sealed class ProjectConfig
    : BaseEntityTypeConfiguration<Project>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Project> builder)
    {
        builder.Property(x => x.Cover)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Technologies)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.MainTechnology)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ProjectType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PrimaryLanguage)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.GitHubUrl)
            .IsRequired(false)
            .HasMaxLength(2048);

        builder.HasMany(x => x.Translations)
            .WithOne()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Gallery)
            .WithOne()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Contributors)
            .WithOne()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Status);
    }
}
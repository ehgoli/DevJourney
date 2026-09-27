using DevJourney.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Courses;

public sealed class EpisodeConfig
    : BaseEntityTypeConfiguration<Episode>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Episode> builder)
    {
        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Caption)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.AttachmentFileName)
            .HasMaxLength(255);

        builder.Property(x => x.VideoFileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.IsFree)
            .IsRequired();

        builder.Property(x => x.VideoDuration)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(x => x.PublishedAt)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.HasIndex(x => x.CourseId);
    }
}
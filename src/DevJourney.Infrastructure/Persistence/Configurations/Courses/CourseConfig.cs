using DevJourney.Domain.Entities.Courses;
using DevJourney.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Courses;

public sealed class CourseConfig
    : BaseEntityTypeConfiguration<Course>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Course> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Cover)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ShortDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Level)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Duration)
            .IsRequired()
            .HasConversion<long>()
            .HasColumnType("bigint");

        builder.Property(x => x.Language)
            .IsRequired()
            .HasConversion(
                language => language.Code,
                code => Language.Create(code))
            .HasMaxLength(2)
            .IsUnicode(false);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.Prerequisites)
            .IsRequired(false);

        builder.Property(x => x.ProductionStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.PublicationStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.ScheduledAt)
            .IsRequired(false)
            .HasColumnType("datetimeoffset");

        builder.HasMany(x => x.Episodes)
            .WithOne()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductionStatus);

        builder.HasIndex(x => x.PublicationStatus);

        builder.HasIndex(x => x.ScheduledAt);
    }
}
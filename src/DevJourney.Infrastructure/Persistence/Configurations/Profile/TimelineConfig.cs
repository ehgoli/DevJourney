using DevJourney.Domain.Entities.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Profile;


public sealed class TimelineConfig
    : BaseEntityTypeConfiguration<Timeline>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Timeline> builder)
    {
        builder.Property(x => x.FromDate)
            .IsRequired();

        builder.Property(x => x.ToDate)
            .IsRequired(false);

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_Timelines_DateRange",
                "[ToDate] IS NULL OR [FromDate] <= [ToDate]"));
        
        builder.HasMany(x => x.Translations)
            .WithOne()
            .HasForeignKey(x => x.TimelineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
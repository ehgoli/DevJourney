using DevJourney.Domain.Entities.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Profile;

public sealed class SkillConfig
    : BaseEntityTypeConfiguration<Skill>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Skill> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Level)
            .IsRequired();

        builder.Property(x => x.YearsOfExperience)
            .IsRequired(false);

        builder.ToTable(table =>
            table.HasCheckConstraint(
                "CK_Skills_Level",
                "[Level] >= 0 AND [Level] <= 100"));
    }
}
using DevJourney.Domain.Entities.Portfolio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations.Portfolio;

public sealed class AchievementConfig
    : BaseEntityTypeConfiguration<Achievement>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Achievement> builder)
    {
        builder.Property(x => x.IssuerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ReceivedDate)
            .IsRequired();

        builder.Property(x => x.ImageFileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CertificateNumber)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.HasMany(x => x.Translations)
            .WithOne()
            .HasForeignKey(x => x.AchievementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ReceivedDate);
    }
}
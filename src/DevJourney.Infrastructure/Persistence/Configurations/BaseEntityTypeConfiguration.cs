using DevJourney.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevJourney.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityTypeConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasQueryFilter(x => !x.IsDeleted);
        
        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(
        EntityTypeBuilder<TEntity> builder);
}
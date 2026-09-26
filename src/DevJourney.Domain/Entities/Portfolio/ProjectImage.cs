using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Portfolio;

public sealed partial class ProjectImage : BaseEntity
{
    public Guid ProjectId { get; private set; }

    public string FileName { get; private set; } = null!;

    public string? Caption { get; private set; }

    public int DisplayOrder { get; private set; }
}
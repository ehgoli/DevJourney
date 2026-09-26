using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Identity;

public sealed partial class UserRole
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }
}
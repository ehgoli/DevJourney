namespace DevJourney.Domain.Entities.Identity;

public partial class UserRole
{
    private UserRole()
    {
    }

    private UserRole(
        Guid userId,
        Guid roleId)
    {
        if (userId == Guid.Empty)
            throw new InvalidOperationException(
                "User ID cannot be empty.");

        if (roleId == Guid.Empty)
            throw new InvalidOperationException(
                "Role ID cannot be empty.");

        UserId = userId;
        RoleId = roleId;
    }

    internal static UserRole Create(
        Guid userId,
        Guid roleId)
    {
        return new UserRole(
            userId: userId,
            roleId: roleId);
    }
}
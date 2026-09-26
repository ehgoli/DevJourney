using DevJourney.Domain.Exceptions;

namespace DevJourney.Domain.Entities.Identity;

public partial class Role
{
    private Role()
    {
    }

    private Role(
        string name,
        string displayName,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new DomainException(
                "Role display name cannot be empty.");

        Name = name.Trim();
        DisplayName = displayName.Trim();
        Description = description?.Trim();
    }

    public static Role Create(
        string name,
        string displayName,
        string? description)
    {
        return new Role(
            name: name,
            displayName: displayName,
            description: description
        );
    }

    public void Modify(
        string displayName,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new DomainException(
                "Role display name cannot be empty.");
        
        DisplayName = displayName.Trim();
        Description = description?.Trim();
    }
}
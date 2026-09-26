using DevJourney.Domain.Exceptions;

namespace DevJourney.Domain.Entities.Identity;

public partial class User
{
    private User()
    {
    }

    private User(
        string fullName,
        string email,
        string phone,
        string passwordHash,
        string? profileImage,
        string? activationCode)
    {
        Validate(
            fullName: fullName,
            email: email,
            phone: phone,
            passwordHash: passwordHash,
            activationCode: activationCode);

        this.FullName = fullName.Trim();
        this.Email = email.Trim();
        this.Phone = phone.Trim();
        this.PasswordHash = passwordHash;
        this.ProfileImage = profileImage?.Trim();
        this.ActivationCode = activationCode?.Trim();

        Status = UserStatus.Active;
    }

    public static User Create(
        string fullName,
        string email,
        string phone,
        string passwordHash,
        string? profileImage,
        string? activationCode)
    {
        return new User(
            fullName: fullName,
            email: email,
            phone: phone,
            passwordHash: passwordHash,
            profileImage: profileImage,
            activationCode: activationCode);
    }

    public void Modify(
        string fullName,
        string email,
        string phone,
        string? profileImage)
    {
        Validate(
            fullName: fullName,
            email: email,
            phone: phone,
            passwordHash: PasswordHash,
            activationCode: ActivationCode);

        FullName = fullName.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
        ProfileImage = profileImage?.Trim();
    }

    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException(
                "Password hash cannot be empty.");

        PasswordHash = passwordHash;
    }

    public void SetActivationCode(string activationCode)
    {
        if (string.IsNullOrWhiteSpace(activationCode))
            throw new DomainException(
                "Activation code cannot be empty.");

        ActivationCode = activationCode.Trim();
    }

    public void ClearActivationCode()
    {
        ActivationCode = null;
    }

    public void Activate()
    {
        if (Status == UserStatus.Banned)
            throw new DomainException(
                "A banned user cannot be activated.");

        if (Status == UserStatus.Active)
            return;

        Status = UserStatus.Active;
    }

    public void Suspend()
    {
        if (Status == UserStatus.Banned)
            throw new DomainException(
                "A banned user cannot be suspended.");

        if (Status == UserStatus.Suspended)
            return;

        Status = UserStatus.Suspended;
    }

    public void Ban()
    {
        if (Status == UserStatus.Banned)
            return;

        Status = UserStatus.Banned;
    }
    
    public void AddRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new DomainException(
                "Role ID cannot be empty.");

        if (_roles.Any(x => x.RoleId == roleId))
            throw new DomainException(
                "User already has this role.");

        _roles.Add(
            UserRole.Create(
                userId: Id,
                roleId: roleId));
    }
    
    public void RemoveRole(Guid roleId)
    {
        var userRole = _roles.FirstOrDefault(
            x => x.RoleId == roleId);

        if (userRole is null)
            throw new DomainException(
                "User does not have this role.");

        _roles.Remove(userRole);
    }
    

    private static void Validate(
        string fullName,
        string email,
        string phone,
        string passwordHash,
        string? activationCode)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException(
                "User full name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException(
                "User email cannot be empty.");

        ValidateEmail(email);

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException(
                "User phone cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException(
                "User password hash cannot be empty.");

        if (activationCode is not null &&
            string.IsNullOrWhiteSpace(activationCode))
        {
            throw new DomainException(
                "Activation code cannot be empty.");
        }
    }

    private static void ValidateEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email.Trim());

            if (!string.Equals(
                    address.Address,
                    email.Trim(),
                    StringComparison.Ordinal))
            {
                throw new DomainException(
                    "User email is invalid.");
            }
        }
        catch (FormatException)
        {
            throw new DomainException(
                "User email is invalid.");
        }
    }
}
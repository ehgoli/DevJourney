namespace DevJourney.Domain.Entities.Identity;

public enum UserStatus
{
    PendingActivationEmail,
    PendingActivationMobile,
    Active,
    Suspended
}
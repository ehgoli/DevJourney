namespace DevJourney.Application.DTOs.Auth.Register;

public enum RegisterError
{
    TooSoon,
    PhoneNumberAlreadyRegistered,
    EmailAlreadyRegistered,
    TooManyAttempts,
    VerificationCodeExpired,
    InvalidVerificationCode
}
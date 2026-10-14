namespace DevJourney.Application.DTOs.Auth.Register;

public record RegisterResponse(bool IsSuccess, RegisterError? Error)
{
    public static RegisterResponse PhoneNumberAlreadyRegistered()
        => new(false, RegisterError.PhoneNumberAlreadyRegistered);

    public static RegisterResponse EmailAlreadyRegistered()
        => new(false, RegisterError.EmailAlreadyRegistered);

    public static RegisterResponse Success()
        => new(true, null);

    public static RegisterResponse TooManyAttempts()
        => new(false, RegisterError.TooManyAttempts);

    public static RegisterResponse VerificationCodeExpired()
        => new(false, RegisterError.VerificationCodeExpired);

    public static RegisterResponse InvalidVerificationCode()
        => new(false, RegisterError.InvalidVerificationCode);
}
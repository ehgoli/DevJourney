namespace DevJourney.Application.DTOs.Auth.Register;

public record RegisterVerificationCodeResponse(bool IsSuccess, RegisterError? Error)
{
    public static RegisterVerificationCodeResponse CodeSent()
        => new(true, null);
    
    public static RegisterVerificationCodeResponse PhoneNumberAlreadyRegistered()
        => new(false, RegisterError.PhoneNumberAlreadyRegistered);

    public static RegisterVerificationCodeResponse TooManyAttempts()
        => new(false, RegisterError.TooManyAttempts);

    public static RegisterVerificationCodeResponse TooSoon(int tryAfterSeconds)
        => new(false, RegisterError.TooSoon);
}
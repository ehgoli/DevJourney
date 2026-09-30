namespace DevJourney.Application.DTOs.Auth.Login;

public sealed record LoginResponse(
    bool IsSuccess,
    LoginError? Error)
{
    public static LoginResponse Success()
        => new(true, null);

    public static LoginResponse NotFound()
        => new(false, LoginError.NotFound);

    public static LoginResponse InvalidCredentials()
        => new(false, LoginError.InvalidCredentials);

    public static LoginResponse Suspended()
        => new(false, LoginError.UserSuspended);

    public static LoginResponse PendingActivation()
        => new(false, LoginError.PendingActivation);

    public static LoginResponse TooManyAttempts()
        => new(false, LoginError.TooManyAttempts);
};
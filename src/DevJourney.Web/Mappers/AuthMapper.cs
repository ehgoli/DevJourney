using DevJourney.Application.DTOs.Auth;
using DevJourney.Application.DTOs.Auth.Login;
using DevJourney.Application.DTOs.Auth.Register;
using DevJourney.Web.ViewModels.Auth;

namespace DevJourney.Web.Mappers;

public static class AuthMapper
{
    public static LoginRequest ToLoginRequest(LoginViewModel model)
    {
        return new LoginRequest(
            Phone: model.Phone, Password: model.Password);
    }

    public static RegisterRequest ToRegisterRequest(RegisterViewModel model)
    {
        return new RegisterRequest(
            FullName:  model.FullName,
            Email:model.Email,
            PhoneNumber: model.PhoneNumber,
            Password:model.Password);
    }

    public static RegisterVerificationCodeRequest ToRegisterVerificationCodeRequest
        (RegisterViewModel model)
    {
        return new RegisterVerificationCodeRequest(PhoneNumber:  model.PhoneNumber);
    }
}
using DevJourney.Application.DTOs.Auth;
using DevJourney.Web.ViewModels.Auth;

namespace DevJourney.Web.Mappers;

public class AuthMapper
{
    public static LoginRequest ToLoginRequest(LoginViewModel model)
    {
        return new LoginRequest(
            Phone: model.Phone, Password: model.Password);
    }
}
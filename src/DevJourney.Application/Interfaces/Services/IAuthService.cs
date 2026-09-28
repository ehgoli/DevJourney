using DevJourney.Application.DTOs.Auth;

namespace DevJourney.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest, bool rememberMe = false);
    Task LogoutAsync();
}
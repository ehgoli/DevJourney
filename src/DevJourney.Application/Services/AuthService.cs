using DevJourney.Application.DTOs.Auth;
using DevJourney.Application.Interfaces.Infrastructure.Identity;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Application.Interfaces.Services;
using DevJourney.Domain.Entities.Identity;
using Microsoft.Extensions.Logging;


namespace DevJourney.Application.Services;

public class AuthService(IUserRepository userRepo, 
    ISignInManager signInManager,
    IPasswordHasher passwordHasher) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(
        LoginRequest loginRequest,
        bool rememberMe = false)
    {
        var user = await userRepo.GetByPhoneAsync(loginRequest.Phone);

        if (user is null)
            return LoginResponse.NotFound();

        if (user.Status == UserStatus.Suspended)
            return LoginResponse.Suspended();

        if (user.Status != UserStatus.Active)
            throw new InvalidOperationException(
                $"Unsupported user status: {user.Status}");

        if (!passwordHasher.Verify(loginRequest.Password, user.PasswordHash))
            return LoginResponse.InvalidCredentials();

        await signInManager.SignInAsync(user, rememberMe);

        return LoginResponse.Success();
    }

    public async Task LogoutAsync() 
        => await signInManager.SignOutAsync();
}
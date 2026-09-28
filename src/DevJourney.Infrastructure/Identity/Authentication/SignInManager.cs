using System.Security.Claims;
using DevJourney.Application.Interfaces.Infrastructure.Identity;
using DevJourney.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;


namespace DevJourney.Infrastructure.Identity.Authentication;

public class SignInManager(IHttpContextAccessor httpContextAccessor) : ISignInManager
{
    private const string AuthenticationScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;

    public async Task SignInAsync(User user, bool rememberMe, CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext
                          ?? throw new InvalidOperationException(
                              "HTTP context is not available.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(
            claims,
            AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = rememberMe
        };

        await httpContext.SignInAsync(
            AuthenticationScheme,
            principal,
            properties);
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext
                          ?? throw new InvalidOperationException(
                              "HTTP context is not available.");

        await httpContext.SignOutAsync(AuthenticationScheme);
    }
}
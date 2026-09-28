using Microsoft.AspNetCore.Authentication.Cookies;

namespace DevJourney.Web.Configuration;


public static class AuthenticationConfiguration
{
    public static IServiceCollection AddAppAuthentication(
        this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.AccessDeniedPath = "/AccessIsDenied";
            options.LoginPath = "/Login";
            options.LogoutPath = "/Logout";

            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(7);

            // options.EventsType = typeof(CustomCookieAuthenticationEvents);
        });

        return services;
    }
}
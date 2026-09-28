using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Application.Interfaces.Services;
using DevJourney.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevJourney.Application;

public static class DependencyInjection
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        #region Register services

        services.AddScoped<IAuthService, AuthService>();

        #endregion
    }
}
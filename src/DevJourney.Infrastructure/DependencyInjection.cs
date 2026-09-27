using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Application.Interfaces.Infrastructure.AI;
using DevJourney.Application.Interfaces.Infrastructure.BackgroundJobs;
using DevJourney.Application.Interfaces.Infrastructure.Caching;
using DevJourney.Application.Interfaces.Infrastructure.Media;
using DevJourney.Application.Interfaces.Infrastructure.Security;
using DevJourney.Application.Interfaces.Infrastructure.Storage;
using DevJourney.Infrastructure.AI;
using DevJourney.Infrastructure.BackgroundJobs;
using DevJourney.Infrastructure.Caching;
using DevJourney.Infrastructure.Media;
using DevJourney.Infrastructure.Security;
using DevJourney.Infrastructure.Storage;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevJourney.Infrastructure;

public static class DependencyInjection
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        #region AI
        
        services.Configure<GeminiOptions>(
            configuration.GetSection("Gemini"));

        services.AddSingleton<IAiModerationService, GeminiModerationService>();
        
        #endregion
        
        #region Background Service

        var hangfireConnectionString = configuration.GetConnectionString("HangfireConnection");

        services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                // config.UseSqlServer(options => options.UseNpgsqlConnection(hangfireConnectionString));

                config.UseFilter(new AutomaticRetryAttribute
                {
                    Attempts = 3, // Maximum of 3 retry attempts.
                    OnAttemptsExceeded = AttemptsExceededAction.Fail, // Moves to Failed after 3 unsuccessful attempts.
                    LogEvents = true // Logs detailed error information.
                });
            }
        );

        services.AddHangfireServer(options => { options.WorkerCount = Environment.ProcessorCount * 5; });

        services.AddScoped<IBackgroundJobService, HangfireJobService>();
        services.AddTransient<RecurringJobsInitializer>();

        // Registers StartupTask to run automatically when the application starts.
        services.AddHostedService<HangfireStartupTask>();

        #endregion

        #region Caching Service

        services.AddMemoryCache();

        services.Configure<CacheOptions>(
            configuration.GetSection(CacheOptions.SectionName));

        services.AddScoped<ICacheService, MemoryCacheService>();

        #endregion

        #region Storage

        services.Configure<StorageOptions>(
            configuration.GetSection("Storage"));

        services.AddSingleton<IFileStorage, LocalFileStorage>();

        #endregion
        
        #region Security

        services.Configure<ClamAvOptions>(
            configuration.GetSection("ClamAV"));

        services.AddSingleton<IFileScanner, ClamAvFileScanner>();

        #endregion

        #region Media

        services.Configure<ImageProcessingOptions>(
            configuration.GetSection("ImageProcessing"));

        services.AddSingleton<IImageProcessor, ImageSharpImageProcessor>();

        #endregion
    }
}
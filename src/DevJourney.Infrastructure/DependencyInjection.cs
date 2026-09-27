using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Application.Interfaces.Infrastructure.AI;
using DevJourney.Application.Interfaces.Infrastructure.BackgroundJobs;
using DevJourney.Application.Interfaces.Infrastructure.Caching;
using DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Email;
using DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Sms;
using DevJourney.Application.Interfaces.Infrastructure.Media;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Articles;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Courses;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Portfolio;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Profile;
using DevJourney.Application.Interfaces.Infrastructure.Security;
using DevJourney.Application.Interfaces.Infrastructure.Storage;
using DevJourney.Domain.Entities.Identity;
using DevJourney.Infrastructure.AI;
using DevJourney.Infrastructure.BackgroundJobs;
using DevJourney.Infrastructure.Caching;
using DevJourney.Infrastructure.ExternalServices.Email;
using DevJourney.Infrastructure.ExternalServices.Sms;
using DevJourney.Infrastructure.Media;
using DevJourney.Infrastructure.Persistence.Context;
using DevJourney.Infrastructure.Persistence.Repositories.Articles;
using DevJourney.Infrastructure.Persistence.Repositories.Courses;
using DevJourney.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Infrastructure.Persistence.Repositories.Portfolio;
using DevJourney.Infrastructure.Persistence.Repositories.Profile;
using DevJourney.Infrastructure.Persistence.Seed;
using DevJourney.Infrastructure.Security;
using DevJourney.Infrastructure.Storage;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevJourney.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region AI
        
        services.AddOptions<GeminiOptions>()
            .Bind(configuration.GetSection(GeminiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IAiModerationService, GeminiModerationService>();
        
        #endregion
        
        #region Background Service

        var hangfireConnectionString = configuration.GetConnectionString("HangfireConnection");

        services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseSqlServerStorage(hangfireConnectionString);

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

        services.AddOptions<CacheOptions>()
            .Bind(configuration.GetSection(CacheOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddScoped<ICacheService, MemoryCacheService>();

        #endregion

        #region Storage

        services.AddOptions<StorageOptions>()
            .Bind(configuration.GetSection(StorageOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IFileStorage, LocalFileStorage>();

        #endregion
        
        #region Security

        services.AddOptions<ClamAvOptions>()
            .Bind(configuration.GetSection(ClamAvOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IFileScanner, ClamAvFileScanner>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        #endregion

        #region Media

        services.AddOptions<ImageProcessingOptions>()
            .Bind(configuration.GetSection(ImageProcessingOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IImageProcessor, ImageSharpImageProcessor>();

        #endregion
        
        #region Persistence
        
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
        });
        
        
        // Aggregate Root repositories
        
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IAchievementRepository, AchievementRepository>();

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        
        #region Seeders

        var seederType = typeof(ISeeder);

        var seederTypes = typeof(DependencyInjection)
            .Assembly
            .GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                seederType.IsAssignableFrom(type));

        foreach (var implementationType in seederTypes)
        {
            services.AddScoped(seederType, implementationType);
        }

        services.AddScoped<DatabaseSeeder>();

        #endregion
        
        #endregion

        #region External Services

        #region Email

        services.AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddScoped<IEmailSender, SmtpEmailSender>();

        #endregion

        #region Sms

        services.AddScoped<ISmsSender, SmsSender>();

        #endregion

        #endregion
    }
}
using System.ComponentModel.DataAnnotations;

namespace DevJourney.Application.Common.Configuration.Identity;

public sealed class PasswordResetOptions
{
    public const string SectionName = "Identity:PasswordReset";

    [Range(1, 20,
        ErrorMessage = "Maximum verification attempts must be between 1 and 20.")]
    public int MaxVerificationAttempts { get; set; } = 5;

    [Range(typeof(TimeSpan), "00:00:30", "1.00:00:00",
        ErrorMessage = "Verification code lifetime must be between 30 seconds and 1 day.")]
    public TimeSpan VerificationCodeLifetime { get; set; } = TimeSpan.FromMinutes(5);

    [Range(typeof(TimeSpan), "00:00:10", "1.00:00:00",
        ErrorMessage = "Resend cooldown must be between 10 seconds and 1 day.")]
    public TimeSpan ResendCooldown { get; set; } = TimeSpan.FromSeconds(60);

    [Range(typeof(TimeSpan), "00:01:00", "7.00:00:00",
        ErrorMessage = "Lock duration must be between 1 minute and 7 days.")]
    public TimeSpan LockDuration { get; set; } = TimeSpan.FromHours(6);

    [Range(typeof(TimeSpan), "00:00:30", "1.00:00:00",
        ErrorMessage = "Reset token lifetime must be between 30 seconds and 1 day.")]
    public TimeSpan ResetTokenLifetime { get; set; } = TimeSpan.FromMinutes(5);
}
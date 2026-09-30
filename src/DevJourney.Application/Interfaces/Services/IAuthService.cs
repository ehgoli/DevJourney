using DevJourney.Application.DTOs.Auth;
using DevJourney.Application.DTOs.Auth.ForgotPassword;
using DevJourney.Application.DTOs.Auth.Login;


namespace DevJourney.Application.Interfaces.Services;

/// <summary>
/// Provides authentication and account security operations.
/// </summary>
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest, bool rememberMe = false,
        CancellationToken cancellationToken = default);

    Task LogoutAsync(CancellationToken cancellationToken = default);


    #region Reset Password

    /// <summary>
    /// Starts the password reset process for the specified phone number.
    /// If the user is eligible, a verification code is generated,
    /// stored temporarily in the cache, and sent to the user's phone.
    /// </summary>
    Task<PasswordResetResponse> RequestPasswordResetAsync(
        PasswordResetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a new verification code for an active password reset process.
    /// The request is subject to the configured resend cooldown and rate limits.
    /// </summary>
    Task<PasswordResetResendCodeResponse> ResendPasswordResetCodeAsync(
        PasswordResetResendCodeRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the verification code submitted by the user.
    /// If the code is valid, a temporary reset token is issued
    /// to authorize changing the user's password.
    /// </summary>
    Task<PasswordResetVerificationResponse> VerifyPasswordResetAsync(
        PasswordResetVerificationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes the password reset process using a valid reset token.
    /// The user's password is replaced with the new password,
    /// and the reset token is invalidated after a successful change.
    /// </summary>
    Task<PasswordResetConfirmationResponse> ConfirmPasswordResetAsync(
        PasswordResetConfirmationRequest request, CancellationToken cancellationToken = default);

    #endregion
}
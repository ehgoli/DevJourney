using System.Security.Cryptography;
using DevJourney.Application.Common.Configuration.Identity;
using DevJourney.Application.DTOs.Auth;
using DevJourney.Application.DTOs.Auth.ForgotPassword;
using DevJourney.Application.DTOs.Auth.Login;
using DevJourney.Application.Interfaces.Infrastructure.Caching;
using DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Sms;
using DevJourney.Application.Interfaces.Infrastructure.Identity;
using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Application.Interfaces.Services;
using DevJourney.Domain.Entities.Identity;
using Microsoft.Extensions.Options;


namespace DevJourney.Application.Services;

public sealed class AuthService(
    IUserRepository userRepo,
    ISignInManager signInManager,
    IPasswordHasher passwordHasher,
    ISmsSender smsSender,
    ICacheService cacheService,
    IOptions<PasswordResetOptions> passwordResetOptions) : IAuthService
{
    private readonly PasswordResetOptions _passwordResetOptions =
        passwordResetOptions.Value;


    #region Login / Logout

    /// <summary>
    /// Authenticates an active user using their phone number and password.
    /// A temporarily locked user cannot start a new authentication attempt.
    /// </summary>
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest,
        bool rememberMe = false, CancellationToken ct = default)
    {
        var user = await userRepo.GetByPhoneAsync(loginRequest.Phone, ct);
        if (user is null)
            return LoginResponse.NotFound();

        if (user.Status == UserStatus.Suspended)
            return LoginResponse.Suspended();

        if (user.Status != UserStatus.Active)
            throw new InvalidOperationException(
                $"Unsupported user status: {user.Status}");
        
        // The temporary security lock is independent of UserStatus.
        // It blocks login and password reset operations for the configured duration.
        if (await IsUserLockedAsync(user.Id, ct))
            return LoginResponse.TooManyAttempts();


        if (!passwordHasher.Verify(loginRequest.Password, user.PasswordHash))
            return LoginResponse.InvalidCredentials();

        await signInManager.SignInAsync(user, rememberMe, ct);

        return LoginResponse.Success();
    }


    /// <summary>
    /// Signs out the currently authenticated user.
    /// </summary>
    public async Task LogoutAsync(
        CancellationToken cancellationToken = default)
        => await signInManager.SignOutAsync(cancellationToken);

    #endregion


    #region Reset Password

    /// <summary>
    /// Starts the password reset flow for the specified phone number.
    ///
    /// Flow:
    /// 1. Find the user.
    /// 2. Reject temporarily locked users.
    /// 3. Validate the user's status.
    /// 4. Generate a five-digit verification code.
    /// 5. Store the verification state in cache.
    /// 6. Send the verification code via SMS.
    /// </summary>
    public async Task<PasswordResetResponse> RequestPasswordResetAsync(
        PasswordResetRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepo.GetByPhoneAsync(request.PhoneNumber, cancellationToken);
        if (user is null)
            return PasswordResetResponse.NotFound();

        // A locked user cannot request a new verification code.
        if (await IsUserLockedAsync(user.Id, cancellationToken))
            return PasswordResetResponse.TooManyAttempts();

        if (user.Status == UserStatus.Suspended)
            return PasswordResetResponse.UserSuspended();

        if (user.Status != UserStatus.Active)
            return PasswordResetResponse.NotFound();


        var cacheKey = GetPasswordResetKey(request.PhoneNumber);

        var existingState = await cacheService.GetAsync<PasswordResetState>(
            cacheKey, cancellationToken);

        // Prevents repeatedly requesting a new code within the configured cooldown.
        if (existingState is not null)
        {
            var now = DateTimeOffset.UtcNow;

            if (now >= existingState.ExpiresAt)
                await cacheService.RemoveAsync(cacheKey,cancellationToken);

            else if (now < existingState.NextResendAt)
                return PasswordResetResponse.ResendTooSoon();
        }

        var code = GenerateVerificationCode();

        var state = new PasswordResetState(
            UserId: user.Id,
            CodeHash: passwordHasher.Hash(code),
            
            ExpiresAt: DateTimeOffset.UtcNow.Add(
                _passwordResetOptions.VerificationCodeLifetime),
            
            NextResendAt: DateTimeOffset.UtcNow.Add(
                _passwordResetOptions.ResendCooldown),

            // Failed attempts belong to the whole reset flow.
            // Resending a code must NOT reset this counter.
            FailedAttempts: existingState?.FailedAttempts ?? 0);

        await cacheService.SetAsync(cacheKey, state,
            _passwordResetOptions.VerificationCodeLifetime,
            cancellationToken: cancellationToken);

        try
        {
            await smsSender.SendAsync(
                new SmsMessage(
                    PhoneNumber: request.PhoneNumber,
                    Message: $"Your password reset verification code is: {code}"),
                cancellationToken: cancellationToken);
        }
        catch
        {
            // Do not leave a valid OTP in cache when SMS delivery fails.
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            throw;
        }

        return PasswordResetResponse.CodeSent();
    }


    /// <summary>
    /// Sends a new verification code for an existing password reset flow.
    ///
    /// The resend operation:
    /// - respects the configured cooldown;
    /// - respects the temporary user lock;
    /// - generates a new OTP;
    /// - preserves the previous failed-attempt count.
    /// </summary>
    public async Task<PasswordResetResendCodeResponse>
        ResendPasswordResetCodeAsync(PasswordResetResendCodeRequest request,
            CancellationToken cancellationToken = default)
    {
        var cacheKey = GetPasswordResetKey(request.PhoneNumber);

        var state = await cacheService.GetAsync<PasswordResetState>(cacheKey, cancellationToken);
        if (state is null)
            return PasswordResetResendCodeResponse.Expired();

        // UserId is stored inside the password reset state,
        // so we can apply the same temporary security lock
        // used across authentication operations.
        if (await IsUserLockedAsync(state.UserId, cancellationToken))
            return PasswordResetResendCodeResponse.TooManyAttempts();


        var user = await userRepo.GetByIdAsync(state.UserId, cancellationToken);
        if (user is null)
            return PasswordResetResendCodeResponse.NotFound();

        if (user.Status == UserStatus.Suspended)
            return PasswordResetResendCodeResponse.UserSuspended();

        if (user.Status != UserStatus.Active)
            return PasswordResetResendCodeResponse.Expired();


        var now = DateTimeOffset.UtcNow;

        if (now >= state.ExpiresAt)
        {
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            return PasswordResetResendCodeResponse.Expired();
        }

        if (now < state.NextResendAt)
        {
            var retryAfter = (int)Math.Ceiling(
                (state.NextResendAt - now).TotalSeconds);
            return PasswordResetResendCodeResponse.TooSoon(retryAfter);
        }

        var code = GenerateVerificationCode();

        var updatedState = state with
        {
            CodeHash = passwordHasher.Hash(code),

            // A new OTP gets a new configured lifetime.
            ExpiresAt = now.Add(
                _passwordResetOptions.VerificationCodeLifetime),

            // The user must wait another configured cooldown before resend.
            NextResendAt = now.Add(
                _passwordResetOptions.ResendCooldown)

            // FailedAttempts intentionally remains unchanged.
        };

        await cacheService.SetAsync(
            cacheKey,
            updatedState,
            _passwordResetOptions.VerificationCodeLifetime,
            cancellationToken: cancellationToken);

        try
        {
            await smsSender.SendAsync(
                new SmsMessage(
                    PhoneNumber: request.PhoneNumber,
                    Message:
                    $"Your password reset verification code is: {code}"),
                cancellationToken);
        }
        catch
        {
            // Do not leave a valid OTP in cache when SMS delivery fails.
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            throw;
        }

        return PasswordResetResendCodeResponse.CodeSent(
            (int)_passwordResetOptions.ResendCooldown.TotalSeconds);
    }


    /// <summary>
    /// Verifies the OTP submitted by the user.
    ///
    /// If the code is valid:
    /// - the OTP state is removed;
    /// - a temporary reset token is generated;
    /// - the reset token is stored for the configured duration.
    ///
    /// If the code is invalid the configured maximum number of times:
    /// - the OTP state is removed;
    /// - the user is temporarily locked.
    /// </summary>
    public async Task<PasswordResetVerificationResponse>
        VerifyPasswordResetAsync(PasswordResetVerificationRequest request,
            CancellationToken cancellationToken = default)
    {
        var user = await userRepo.GetByPhoneAsync(request.PhoneNumber, cancellationToken);
        if (user is null)
            return PasswordResetVerificationResponse.Expired();

        // The user lock is checked before verifying the OTP.
        // A locked user cannot continue the password reset flow.
        if (await IsUserLockedAsync(user.Id, cancellationToken))
            return PasswordResetVerificationResponse.TooManyAttempts();

        var cacheKey = GetPasswordResetKey(request.PhoneNumber);

        var state = await cacheService.GetAsync<PasswordResetState>(cacheKey, cancellationToken);
        if (state is null)
            return PasswordResetVerificationResponse.Expired();

        var now = DateTimeOffset.UtcNow;

        // Remove expired OTP state immediately.
        if (now >= state.ExpiresAt)
        {
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            return PasswordResetVerificationResponse.Expired();
        }

        // Protect against an inconsistent cache state.
        if (state.FailedAttempts >=
            _passwordResetOptions.MaxVerificationAttempts)
        {
            await cacheService.RemoveAsync(cacheKey, cancellationToken);

            await LockUserAsync(user.Id, cancellationToken);

            return PasswordResetVerificationResponse.TooManyAttempts();
        }

        bool isValid = passwordHasher.Verify(request.Code, state.CodeHash);

        if (!isValid)
        {
            int failedAttempts = state.FailedAttempts + 1;

            // The maximum number of attempts has been reached.
            // Remove the OTP and temporarily lock the user.
            if (failedAttempts >= _passwordResetOptions.MaxVerificationAttempts)
            {
                await cacheService.RemoveAsync(cacheKey, cancellationToken);

                await LockUserAsync(user.Id, cancellationToken);

                return PasswordResetVerificationResponse.TooManyAttempts();
            }

            var updatedState = state with
            {
                FailedAttempts = failedAttempts
            };

            // Preserve the original OTP expiration time.
            var remainingLifetime = state.ExpiresAt - now;

            if (remainingLifetime > TimeSpan.Zero)
            {
                await cacheService.SetAsync(
                    cacheKey,
                    updatedState,
                    remainingLifetime,
                    cancellationToken: cancellationToken);
            }

            return PasswordResetVerificationResponse.InvalidCode();
        }

        // The OTP has been verified successfully.
        // It must not be reusable.
        await cacheService.RemoveAsync(cacheKey, cancellationToken);

        // Issue a short-lived token that authorizes the password change.
        var resetToken = Convert.ToHexString(
            RandomNumberGenerator.GetBytes(32));

        var tokenState = new PasswordResetTokenState(
            UserId: user.Id,
            ExpiresAt: now.Add(
                _passwordResetOptions.ResetTokenLifetime));

        await cacheService.SetAsync(
            GetResetTokenKey(resetToken),
            tokenState,
            _passwordResetOptions.ResetTokenLifetime,
            cancellationToken: cancellationToken);

        return PasswordResetVerificationResponse.Verified(resetToken);
    }


    /// <summary>
    /// Completes the password reset using a valid reset token.
    ///
    /// The reset token:
    /// - is valid for the configured duration;
    /// - is only usable after successful OTP verification;
    /// - cannot be used once the user is temporarily locked;
    /// - is removed after a successful password change.
    /// </summary>
    public async Task<PasswordResetConfirmationResponse>
        ConfirmPasswordResetAsync(PasswordResetConfirmationRequest request,
            CancellationToken cancellationToken = default)
    {
        var cacheKey = GetResetTokenKey(request.ResetToken);

        var tokenState = await cacheService.GetAsync<PasswordResetTokenState>(
                cacheKey, cancellationToken);
        if (tokenState is null)
            return PasswordResetConfirmationResponse.InvalidToken();

        if (await IsUserLockedAsync(tokenState.UserId, cancellationToken))
            return PasswordResetConfirmationResponse.InvalidToken();

        var now = DateTimeOffset.UtcNow;

        if (now >= tokenState.ExpiresAt)
        {
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            return PasswordResetConfirmationResponse.InvalidToken();
        }

        var user = await userRepo.GetByIdAsync(tokenState.UserId, cancellationToken);
        if (user is null)
        {
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            return PasswordResetConfirmationResponse.NotFound();
        }

        if (user.Status != UserStatus.Active)
        {
            await cacheService.RemoveAsync(cacheKey, cancellationToken);
            return PasswordResetConfirmationResponse.InvalidToken();
        }

        var passwordHash = passwordHasher.Hash(request.NewPassword);

        user.ChangePassword(passwordHash);

        await userRepo.UpdateAsync(user, cancellationToken);

        // Reset token is single-use.
        await cacheService.RemoveAsync(cacheKey, cancellationToken);

        return PasswordResetConfirmationResponse.Success();
    }

    #endregion


    #region User Authentication Lock

    /// <summary>
    /// Checks whether the user currently has a temporary authentication lock.
    /// The lock is stored in cache and automatically expires after the configured duration.
    /// </summary>
    private async Task<bool> IsUserLockedAsync(
        Guid userId, CancellationToken cancellationToken = default)
        => await cacheService.GetAsync<bool>(GetUserLockKey(userId), cancellationToken);


    /// <summary>
    /// Temporarily blocks authentication-related operations for the user.
    /// This is intentionally separate from UserStatus.Suspended because
    /// the lock is temporary and security-related.
    /// </summary>
    private async Task LockUserAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        await cacheService.SetAsync(
            GetUserLockKey(userId),
            true,
            _passwordResetOptions.LockDuration,
            cancellationToken: cancellationToken);
    }

    #endregion


    #region Helpers

    private static string GenerateVerificationCode()
        => RandomNumberGenerator
            .GetInt32(10_000, 100_000)
            .ToString("D5");

    private static string GetPasswordResetKey(string phoneNumber)
        => $"password-reset:{phoneNumber}";

    private static string GetResetTokenKey(string resetToken)
        => $"reset-token:{resetToken}";

    private static string GetUserLockKey(Guid userId)
        => $"user-lock:{userId}";

    #endregion
}
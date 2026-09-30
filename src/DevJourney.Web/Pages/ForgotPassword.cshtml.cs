using DevJourney.Application.DTOs.Auth;
using DevJourney.Application.DTOs.Auth.ForgotPassword;
using DevJourney.Application.Interfaces.Services;
using DevJourney.Web.ViewModels;
using DevJourney.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace DevJourney.Web.Pages;

public class ForgotPasswordModel(
    IAuthService authService,
    IStringLocalizer<SharedResource> localizer) : PageModel
{
    [BindProperty]
    public ForgotPasswordViewModel ForgotPassword { get; set; } = new();

    public void OnGet()
    {
        ForgotPassword.CurrentStep = 1;
    }

    public async Task<IActionResult> OnPostRequestCodeAsync(
        CancellationToken cancellationToken)
    {
        var request = new PasswordResetRequest(ForgotPassword.PhoneNumber);

        var response = await authService.RequestPasswordResetAsync(request, cancellationToken);

        if (!response.IsSuccess)
        {
            ForgotPassword.CurrentStep = 1;
            ForgotPassword.ErrorMessage = MapError(response.Error);

            return Page();
        }

        ForgotPassword.CurrentStep = 2;

        return Page();
    }

    public async Task<IActionResult> OnPostResendCodeAsync(
        CancellationToken cancellationToken)
    {
        var request = new PasswordResetResendCodeRequest(ForgotPassword.PhoneNumber);

        var response = await authService.ResendPasswordResetCodeAsync(request,cancellationToken);

        if (!response.Success)
        {
            ForgotPassword.CurrentStep = 2;
            ForgotPassword.ErrorMessage = MapError(response.Error);

            return Page();
        }

        ForgotPassword.CurrentStep = 2;

        return Page();
    }

    public async Task<IActionResult> OnPostVerifyCodeAsync(
        CancellationToken cancellationToken)
    {
        var request = new PasswordResetVerificationRequest(
            PhoneNumber: ForgotPassword.PhoneNumber,
            Code: ForgotPassword.Code);

        var response = await authService.VerifyPasswordResetAsync(request, cancellationToken);

        if (!response.Success)
        {
            ForgotPassword.CurrentStep = 2;
            ForgotPassword.ErrorMessage = MapError(response.Error);

            return Page();
        }

        ForgotPassword.ResetToken = response.ResetToken;
        ForgotPassword.CurrentStep = 3;

        return Page();
    }

    public async Task<IActionResult> OnPostConfirmResetAsync(
        CancellationToken cancellationToken)
    {
        if (ForgotPassword.NewPassword != ForgotPassword.ConfirmPassword)
        {
            ForgotPassword.CurrentStep = 3;
            ForgotPassword.ErrorMessage =
                localizer["PasswordsDoNotMatch"].Value;

            return Page();
        }

        if (string.IsNullOrWhiteSpace(ForgotPassword.ResetToken))
        {
            ForgotPassword.CurrentStep = 1;
            ForgotPassword.ErrorMessage =
                localizer["InvalidResetToken"].Value;

            return Page();
        }

        var request = new PasswordResetConfirmationRequest(
            ForgotPassword.ResetToken,
            ForgotPassword.NewPassword);

        var response = await authService.ConfirmPasswordResetAsync(request, cancellationToken);

        if (!response.IsSuccess)
        {
            ForgotPassword.CurrentStep = 3;
            ForgotPassword.ErrorMessage = MapError(response.Error);

            return Page();
        }

        ForgotPassword.CurrentStep = 4;

        return Page();
    }

    private string MapError(PasswordResetError? error)
    {
        return error switch
        {
            PasswordResetError.NotFound =>
                localizer["UserNotFound"].Value,

            PasswordResetError.UserSuspended =>
                localizer["UserSuspended"].Value,

            PasswordResetError.InvalidCode =>
                localizer["InvalidVerificationCode"].Value,

            PasswordResetError.Expired =>
                localizer["VerificationCodeExpired"].Value,

            PasswordResetError.TooManyAttempts =>
                localizer["TooManyVerificationAttempts"].Value,

            PasswordResetError.ResendTooSoon =>
                localizer["ResendCodeTooSoon"].Value,

            PasswordResetError.InvalidToken =>
                localizer["InvalidResetToken"].Value,

            _ => localizer["UnexpectedError"].Value
        };
    }
}
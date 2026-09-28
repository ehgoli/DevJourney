using DevJourney.Application.DTOs.Auth;
using DevJourney.Application.Interfaces.Services;
using DevJourney.Web.Mappers;
using DevJourney.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace DevJourney.Web.Pages;

public sealed class LoginModel(
    IAuthService authService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<LoginModel> logger)  : PageModel
{
    public void OnGet()
    {
    }

    [BindProperty]
    public LoginViewModel LoginInputs { get; set; } = new();
    
    public async Task<IActionResult> OnPostAsync(string? returnUrl = "/")
    {
        if (!Url.IsLocalUrl(returnUrl))
        {
            logger.LogWarning(
                "Invalid return URL received during login. ReturnUrl: {ReturnUrl}",
                returnUrl);
            
            returnUrl = "/";
        }

        if (!ModelState.IsValid)
            return Page();

        var request = AuthMapper.ToLoginRequest(LoginInputs);

        var response = await authService.LoginAsync(request, LoginInputs.RememberMe);

        if(response.IsSuccess)    
            return Redirect(returnUrl);
        
        LoginInputs.ErrorMessage = response.Error switch
        {
            LoginError.NotFound =>
                localizer["UserNotFound"],

            LoginError.InvalidCredentials =>
                localizer["InvalidCredentials"],

            LoginError.UserSuspended =>
                localizer["UserSuspended"],

            LoginError.PendingActivation =>
                localizer["PendingActivation"],

            _ =>
                localizer["UnexpectedError"]
        };
        
        return Page();
    }
}
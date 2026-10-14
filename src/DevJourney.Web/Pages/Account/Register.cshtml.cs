using DevJourney.Application.DTOs.Auth.Register;
using DevJourney.Application.Interfaces.Services;
using DevJourney.Web.Mappers;
using DevJourney.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace DevJourney.Web.Pages.Account;

public sealed class RegisterModel(IAuthService authService,
    IStringLocalizer<SharedResource> localizer) : PageModel
{

    [BindProperty]
    public RegisterViewModel RegisterInputs { get; set; } = new();


    public void OnGet()
    {
        RegisterInputs.IsVerificationCodeSent = false;
        RegisterInputs.ErrorMessage = null;
    }


    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var registerRequest = AuthMapper.ToRegisterRequest(RegisterInputs);

        var registerResponse = await authService.RegisterAsync(registerRequest,
                cancellationToken);

        if (registerResponse.IsSuccess)
        {
            TempData["RegistrationCompleted"] = true;
            TempData["RegistrationName"] = RegisterInputs.FullName;
            TempData["RegistrationEmail"] = RegisterInputs.Email;

            return RedirectToPage("./SuccessRegister");
        }

        RegisterInputs.ErrorMessage = MapError(registerResponse.Error);
        RegisterInputs.IsVerificationCodeSent = true;
        return Page();
    }

    
    private string MapError(RegisterError? error)
    {
        return error switch
        {
            RegisterError.PhoneNumberAlreadyRegistered
                => localizer["PhoneNumberAlreadyRegistered"],

            RegisterError.EmailAlreadyRegistered
                => localizer["EmailAlreadyRegistered"],

            _ => localizer["UnexpectedError"]
        };
    }
}
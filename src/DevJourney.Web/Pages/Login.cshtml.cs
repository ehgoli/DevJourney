using DevJourney.Application.Interfaces.Services;
using DevJourney.Web.Mappers;
using DevJourney.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevJourney.Web.Pages;

public class LoginModel(IAuthService authService) : PageModel
{
    public async Task OnGetAsync()
    {
    }


    public async Task<IActionResult> OnPostAsync(LoginViewModel model)
    {
        var result = await authService.LoginAsync(
            AuthMapper.ToLoginRequest(model));

        throw new NotImplementedException();
    }
}
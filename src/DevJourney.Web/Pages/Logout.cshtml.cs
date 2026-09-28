using DevJourney.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevJourney.Web.Pages;

public class LogoutModel(IAuthService authService) : PageModel
{
    public async Task<RedirectResult> OnGetAsync()
    {
        await authService.LogoutAsync();
        return Redirect("/Login");
    }
}
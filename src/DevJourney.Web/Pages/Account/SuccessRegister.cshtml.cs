using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevJourney.Web.Pages.Account;

public class SuccessRegister : PageModel
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    
    public IActionResult OnGet()
    {
        if (TempData["RegistrationCompleted"] is not true)
            return RedirectToPage("./Register");

        
        this.UserName = TempData["RegistrationName"]?.ToString() ?? string.Empty;
        this.Email = TempData["RegistrationEmail"]?.ToString() ?? string.Empty;

        return Page();
    }
}
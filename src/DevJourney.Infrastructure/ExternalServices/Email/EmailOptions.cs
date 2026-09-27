using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.ExternalServices.Email;

public class EmailOptions
{
    public string SmtpHost { get; set; } = string.Empty;

    public int SmtpPort { get; set; } = 587; 

    public bool EnableSsl { get; set; } = true;

    public string FromName { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
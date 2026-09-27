namespace DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Email;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message,
        CancellationToken cancellationToken = default);
}
using System.Net;
using System.Net.Mail;
using DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Email;
using Microsoft.Extensions.Options;


namespace DevJourney.Infrastructure.ExternalServices.Email;

public sealed class SmtpEmailSender(IOptions<EmailOptions> options) : IEmailSender
{
    private readonly EmailOptions _settings = options.Value;

    public async Task SendAsync(
        EmailMessage emailMessage,
        CancellationToken cancellationToken = default)
    {
        using var mail = new MailMessage
        {
            From = new MailAddress(
                _settings.FromEmail,
                _settings.FromName),

            Subject = emailMessage.Subject,
            Body = emailMessage.Body,
            IsBodyHtml = true
        };

        mail.To.Add(emailMessage.Recipient);

        if (!string.IsNullOrWhiteSpace(emailMessage.AttachmentPath))
        {
            mail.Attachments.Add(
                new Attachment(emailMessage.AttachmentPath));
        }

        using var smtpClient = new SmtpClient(_settings.SmtpHost)
        {
            Port = _settings.SmtpPort,
            Credentials = new NetworkCredential(
                _settings.FromEmail,
                _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        await smtpClient.SendMailAsync(
            mail,
            cancellationToken);
    }
}
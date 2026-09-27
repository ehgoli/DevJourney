namespace DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Sms;

public interface ISmsSender
{
    Task SendAsync(
        SmsMessage message,
        CancellationToken cancellationToken = default);
}
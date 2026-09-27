using DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Sms;

namespace DevJourney.Infrastructure.ExternalServices.Sms;

public class SmsSender : ISmsSender
{
    public Task SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
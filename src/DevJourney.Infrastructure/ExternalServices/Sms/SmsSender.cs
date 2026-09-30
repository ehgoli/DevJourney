using DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Sms;
using Microsoft.Extensions.Logging;

namespace DevJourney.Infrastructure.ExternalServices.Sms;

public class SmsSender(ILogger<SmsSender> logger) : ISmsSender
{
    public async Task SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => logger.LogInformation($"SMS: {message.PhoneNumber} -> {message.Message}"), cancellationToken);
    }
}
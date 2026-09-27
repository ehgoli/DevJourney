namespace DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Sms;

public sealed record SmsMessage(string PhoneNumber, string Message);
namespace DevJourney.Application.Interfaces.Infrastructure.ExternalServices.Email;

public sealed record EmailMessage(
    string Recipient,
    string Subject,
    string Body,
    string? AttachmentPath = null);
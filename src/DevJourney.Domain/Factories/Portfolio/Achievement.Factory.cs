using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Portfolio;

public partial class Achievement
{
    private Achievement()
    {
    }
    
    private Achievement(
        string issuerName,
        DateOnly receivedDate,
        string imageFileName,
        string? certificateNumber = null)
    {
        if (string.IsNullOrWhiteSpace(issuerName))
            throw new DomainException(
                "Issuer name cannot be empty.");

        if (receivedDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException(
                "Received date cannot be in the future.");

        if (string.IsNullOrWhiteSpace(imageFileName))
            throw new DomainException(
                "Image file name cannot be empty.");

        this.IssuerName = issuerName.Trim();
        this.ReceivedDate = receivedDate;
        this.ImageFileName = imageFileName.Trim();
        this.CertificateNumber = certificateNumber?.Trim();
    }
    
    public static Achievement Create(
        string issuerName,
        DateOnly receivedDate,
        string imageFileName,
        string? certificateNumber = null)
    {
        return new Achievement(
            issuerName: issuerName,
            receivedDate: receivedDate,
            imageFileName: imageFileName,
            certificateNumber: certificateNumber);
    }

    public void Modify(
        string issuerName,
        DateOnly receivedDate,
        string imageFileName,
        string? certificateNumber = null)
    {
        if (string.IsNullOrWhiteSpace(issuerName))
            throw new DomainException(
                "Issuer name cannot be empty.");

        if (receivedDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException(
                "Received date cannot be in the future.");

        if (string.IsNullOrWhiteSpace(imageFileName))
            throw new DomainException(
                "Image file name cannot be empty.");
        
        this.IssuerName = issuerName.Trim();
        this.ReceivedDate = receivedDate;
        this.ImageFileName = imageFileName.Trim();
        this.CertificateNumber = certificateNumber?.Trim();
    }
    
    public void AddTranslation(
        Language language,
        string title,
        string? description)
    {
        if (_translations.Any(x => x.Language == language))
        {
            throw new DomainException(
                $"A translation for '{language}' already exists.");
        }

        var translation = AchievementTranslation.Create(
            achievementId: Id,
            language: language,
            title: title,
            description: description);

        _translations.Add(translation);
    }
}
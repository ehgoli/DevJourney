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
            throw new InvalidOperationException(
                "Issuer name cannot be empty.");

        if (receivedDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new InvalidOperationException(
                "Received date cannot be in the future.");

        if (string.IsNullOrWhiteSpace(imageFileName))
            throw new InvalidOperationException(
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
            throw new InvalidOperationException(
                "Issuer name cannot be empty.");

        if (receivedDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new InvalidOperationException(
                "Received date cannot be in the future.");

        if (string.IsNullOrWhiteSpace(imageFileName))
            throw new InvalidOperationException(
                "Image file name cannot be empty.");
        
        this.IssuerName = issuerName.Trim();
        this.ReceivedDate = receivedDate;
        this.ImageFileName = imageFileName.Trim();
        this.CertificateNumber = certificateNumber?.Trim();
    }
    
    public void AddTranslation(
        string language,
        string title,
        string? description)
    {
        if (_translations.Any(x =>
                string.Equals(
                    x.Language,
                    language,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
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
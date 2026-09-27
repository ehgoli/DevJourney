using DevJourney.Domain.Exceptions;
using DevJourney.Domain.ValueObjects;

namespace DevJourney.Domain.Entities.Profile;

public sealed partial class Profile
{
    private Profile()
    {
    }

    private Profile(
        string email,
        string phone)
    {
        this.Email = email;
        this.Phone = phone;
    }

    public static Profile Create(
        string email,
        string phone)
    {
        ValidateContactInformation(
            email: email,
            phone: phone);

        return new Profile(
            email: email,
            phone: phone);
    }

    public void Modify(
        string email,
        string phone)
    {
        ValidateContactInformation(
            email: email,
            phone: phone);

        this.Email = email;
        this.Phone = phone;
    }

    public void AddSkill(
        string name,
        int level,
        int? yearsOfExperience = null)
    {
        _skills.Add(
            Skill.Create(
                name: name,
                level: level,
                yearsOfExperience: yearsOfExperience));
    }

    public void AddTimeline(
        DateOnly fromDate,
        DateOnly? toDate)
    {
        this._timeline.Add(
            Timeline.Create(
                fromDate: fromDate,
                toDate: toDate));
    }

    public void AddSocialMedia(
        string title,
        string url,
        string icon,
        int displayOrder)
    {
        this._socialMedia.Add(
            SocialMedia.Create(
                title: title,
                url: url,
                icon: icon,
                displayOrder: displayOrder));
    }

    public void AddTranslation(
        Language language,
        string fullName,
        string heading,
        string bio,
        string about)
    {
        if (_translations.Any(x => x.Language == language))
        {
            throw new DomainException(
                $"A translation for language '{language}' already exists.");
        }

        this._translations.Add(
            ProfileTranslation.Create(
                profileId: Id,
                language: language,
                fullName: fullName,
                heading: heading,
                bio: bio,
                about: about));
    }

    private static void ValidateContactInformation(
        string email,
        string phone)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException(
                "Email cannot be empty.");
        }

        if (email.Length > 320)
        {
            throw new DomainException(
                "Email cannot exceed 320 characters.");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new DomainException(
                "Phone cannot be empty.");
        }

        if (phone.Length > 20)
        {
            throw new DomainException(
                "Phone cannot exceed 20 characters.");
        }
    }
}
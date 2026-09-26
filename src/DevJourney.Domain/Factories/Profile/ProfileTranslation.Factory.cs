using DevJourney.Domain.Entities.Common;
using DevJourney.Domain.ValueObjects;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class ProfileTranslation
{
    private ProfileTranslation()
    {
    }

    public ProfileTranslation(Guid profileId, 
        Language language, 
        string fullName, 
        string heading, 
        string bio, 
        string about)
    {
        this.ProfileId = profileId;
        this.Language = language;
        this.FullName = fullName;
        this.Heading = heading;
        this.Bio = bio;
        this.About = about;
    }

    public static ProfileTranslation Create(Guid profileId, 
            Language language, 
            string fullName, 
            string heading, 
            string bio, 
            string about)
    {
        return new ProfileTranslation(
            profileId: profileId, 
            language: language, 
            fullName: fullName,
            heading: heading, 
            bio: bio, 
            about: about);
    }
    
    public void Modify(string fullName, 
        string heading, 
        string bio, 
        string about)
    {
        this.FullName = fullName;
        this.Heading = heading;
        this.Bio = bio;
        this.About = about;
    }
}


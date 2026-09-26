using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class Profile : BaseEntity
{
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;


    private readonly List<ProfileTranslation> _translations = [];
    private readonly List<Skill> _skills = [];
    private readonly List<Timeline> _timeline = [];
    private readonly List<SocialMedia> _socialMedia = [];

    public IReadOnlyCollection<ProfileTranslation> Translations => _translations;
    public IReadOnlyCollection<Skill> Skills => _skills;
    public IReadOnlyCollection<Timeline> Timeline => _timeline;
    public IReadOnlyCollection<SocialMedia> SocialMedia => _socialMedia;
}
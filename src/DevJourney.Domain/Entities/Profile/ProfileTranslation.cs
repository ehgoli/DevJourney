using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.ValueObjects;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class ProfileTranslation : BaseEntity
{
    public Guid ProfileId { get; private set; }

    public Language Language { get; private set; }

    public string FullName { get; private set; } = null!;
    public string Heading { get; private set; } = null!;
    public string Bio { get; private set; } = null!;
    public string About { get; private set; } = null!;
}
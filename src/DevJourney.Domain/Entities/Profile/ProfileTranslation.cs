using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class ProfileTranslation : BaseEntity
{
    public Guid ProfileId { get; private set; }

    public string Language { get; private set; } = null!;

    public string FullName { get; private set; } = null!;
    public string Heading { get; private set; } = null!;
    public string Bio { get; private set; } = null!;
    public string About { get; private set; } = null!;
}
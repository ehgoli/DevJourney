using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevJourney.Domain.Entities.Profile;

public sealed partial class SocialMedia : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public string Icon { get; private set; } = null!;
    public int DisplayOrder { get; private set; }
}
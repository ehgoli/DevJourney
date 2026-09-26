using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Identity;

public sealed partial class Role : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string DisplayName { get; private set; } = null!;

    public string? Description { get; private set; }
}
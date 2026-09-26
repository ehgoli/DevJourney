using DevJourney.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class Skill : BaseEntity
{
    public string Name { get; private set; } = null!;
    public int Level { get; private set; }
    public int? YearsOfExperience { get; private set; }
}
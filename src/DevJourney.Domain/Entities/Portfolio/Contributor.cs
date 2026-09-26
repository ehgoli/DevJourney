using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;


namespace DevJourney.Domain.Entities.Portfolio;

public sealed partial class Contributor : BaseEntity
{
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = null!;

    public string Heading { get; private set; } = null!;

    public string GithubUrl { get; private set; } = null!;

    public string Image { get; private set; } = null!;
}
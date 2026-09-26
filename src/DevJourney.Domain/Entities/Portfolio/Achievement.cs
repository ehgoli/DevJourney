using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Portfolio;

public sealed partial class Achievement : BaseEntity
{
    public string IssuerName { get; private set; } = null!;

    public DateOnly ReceivedDate { get; private set; }

    public string ImageFileName { get; private set; } = null!;

    public string? CertificateNumber { get; private set; }

    private readonly List<AchievementTranslation> _translations = [];

    public IReadOnlyCollection<AchievementTranslation> Translations =>
        _translations;
}
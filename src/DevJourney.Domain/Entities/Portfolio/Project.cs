using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Portfolio;

public sealed partial class Project : BaseEntity
{
    public string Cover { get; private set; } = null!;

    public string? Technologies { get; private set; }

    public ProjectStatus Status { get; private set; }

    public string MainTechnology { get; private set; } = null!;

    public string ProjectType { get; private set; } = null!;

    public string PrimaryLanguage { get; private set; } = null!;

    public string? GitHubUrl { get; private set; }

    public IReadOnlyCollection<ProjectTranslation> Translations => _translations;

    public IReadOnlyCollection<ProjectImage> Gallery => _gallery;

    public IReadOnlyCollection<Contributor> Contributors => _contributors;

    private readonly List<ProjectTranslation> _translations = [];
    private readonly List<ProjectImage> _gallery = [];
    private readonly List<Contributor> _contributors = [];
}
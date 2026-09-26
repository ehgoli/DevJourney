namespace DevJourney.Domain.Entities.Portfolio;

public partial class Project
{
    
    #region Constructor

    private Project()
    {
    }

    private Project(
        string cover,
        string? technologies,
        ProjectStatus status,
        string mainTechnology,
        string projectType,
        string primaryLanguage,
        string? gitHubUrl)
    {
        Validate(
            cover,
            technologies,
            status,
            mainTechnology,
            projectType,
            primaryLanguage,
            gitHubUrl);

        Cover = cover.Trim();
        Technologies = NormalizeTechnologies(technologies);
        Status = status;
        MainTechnology = mainTechnology.Trim();
        ProjectType = projectType.Trim();
        PrimaryLanguage = primaryLanguage.Trim();
        GitHubUrl = NormalizeGitHubUrl(gitHubUrl);
    }

    #endregion

    #region Creation

    public static Project Create(
        string cover,
        string? technologies,
        ProjectStatus status,
        string mainTechnology,
        string projectType,
        string primaryLanguage,
        string? gitHubUrl)
    {
        return new Project(
            cover: cover,
            technologies: technologies,
            status: status,
            mainTechnology: mainTechnology,
            projectType: projectType,
            primaryLanguage: primaryLanguage,
            gitHubUrl: gitHubUrl);
    }

    #endregion

    #region Modification

    public void Modify(
        string cover,
        string? technologies,
        ProjectStatus status,
        string mainTechnology,
        string projectType,
        string primaryLanguage,
        string? gitHubUrl)
    {
        Validate(
            cover,
            technologies,
            status,
            mainTechnology,
            projectType,
            primaryLanguage,
            gitHubUrl);

        Cover = cover.Trim();
        Technologies = NormalizeTechnologies(technologies);
        Status = status;
        MainTechnology = mainTechnology.Trim();
        ProjectType = projectType.Trim();
        PrimaryLanguage = primaryLanguage.Trim();
        GitHubUrl = NormalizeGitHubUrl(gitHubUrl);
    }

    #endregion

    
    #region Project Status

    public void ChangeStatus(ProjectStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new InvalidOperationException(
                "Invalid project status.");

        Status = status;
    }

    public void StartDevelopment()
    {
        ChangeStatus(ProjectStatus.InDevelopment);
    }

    public void StartTesting()
    {
        ChangeStatus(ProjectStatus.Testing);
    }

    public void Complete()
    {
        ChangeStatus(ProjectStatus.Completed);
    }

    public void StartRefactoring()
    {
        ChangeStatus(ProjectStatus.Refactoring);
    }

    #endregion

    #region Translations

    public void AddTranslation(
        string language,
        string title,
        string shortDescription,
        string description)
    {
        if (_translations.Any(x =>
                string.Equals(
                    x.Language.Trim(),
                    language.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "A translation for this language already exists.");
        }

        var translation = ProjectTranslation.Create(
            projectId: Id,
            language: language,
            title: title,
            shortDescription: shortDescription,
            description: description);

        _translations.Add(translation);
    }

    #endregion

    #region Images

    public void AddImage(
        string fileName,
        string? caption,
        int displayOrder)
    {
        var image = ProjectImage.Create(
            projectId: this.Id,
            fileName: fileName,
            caption: caption,
            displayOrder: displayOrder);

        _gallery.Add(image);
    }

    #endregion

    #region Contributors

    public void AddContributor(
        string name,
        string heading,
        string gitHubUrl,
        string image)
    {
        var contributor = Contributor.Create(
            projectId: Id,
            name: name,
            heading: heading,
            githubUrl: gitHubUrl,
            image: image);

        _contributors.Add(contributor);
    }

    #endregion

    
    #region Validation

    private static void Validate(
        string cover,
        string? technologies,
        ProjectStatus status,
        string mainTechnology,
        string projectType,
        string primaryLanguage,
        string? gitHubUrl)
    {
        if (string.IsNullOrWhiteSpace(cover))
            throw new InvalidOperationException(
                "Cover cannot be empty.");

        if (!Enum.IsDefined(status))
            throw new InvalidOperationException(
                "Invalid project status.");

        if (string.IsNullOrWhiteSpace(mainTechnology))
            throw new InvalidOperationException(
                "Main technology cannot be empty.");

        if (string.IsNullOrWhiteSpace(projectType))
            throw new InvalidOperationException(
                "Project type cannot be empty.");

        if (string.IsNullOrWhiteSpace(primaryLanguage))
            throw new InvalidOperationException(
                "Primary language cannot be empty.");

        ValidateGitHubUrl(gitHubUrl);
    }

    private static void ValidateGitHubUrl(string? gitHubUrl)
    {
        if (string.IsNullOrWhiteSpace(gitHubUrl))
            return;

        if (!Uri.TryCreate(
                gitHubUrl.Trim(),
                UriKind.Absolute,
                out var uri))
        {
            throw new InvalidOperationException(
                "GitHub URL is invalid.");
        }

        if (uri.Scheme != Uri.UriSchemeHttp &&
            uri.Scheme != Uri.UriSchemeHttps)
            throw new InvalidOperationException(
                "GitHub URL must use HTTP or HTTPS.");

        if (!uri.Host.Equals(
                "github.com",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "URL must belong to GitHub.");
        }
    }

    #endregion

    #region Normalization

    private static string? NormalizeTechnologies(string? technologies)
    {
        if (string.IsNullOrWhiteSpace(technologies))
            return null;

        var items = technologies
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return items.Length == 0
            ? null
            : string.Join(", ", items);
    }

    private static string? NormalizeGitHubUrl(string? gitHubUrl)
    {
        return string.IsNullOrWhiteSpace(gitHubUrl)
            ? null
            : gitHubUrl.Trim();
    }

    #endregion
}
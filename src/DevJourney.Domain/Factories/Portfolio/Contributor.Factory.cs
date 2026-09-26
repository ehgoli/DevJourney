using DevJourney.Domain.Exceptions;

namespace DevJourney.Domain.Entities.Portfolio;

public partial class Contributor
{
    private Contributor()
    {
    }

    private Contributor(Guid projectId, string name, string heading, string githubUrl, string image)
    {
        Validate(
            projectId,
            name,
            heading,
            githubUrl,
            image);
        
        this.ProjectId = projectId;
        this.Name = name;
        this.Heading = heading;
        this.GithubUrl = githubUrl;
        this.Image = image;
    }

    internal static Contributor Create(Guid projectId, string name, string heading, string githubUrl, string image)
    {
        return new Contributor(projectId: projectId,
            name: name,
            heading: heading,
            githubUrl: githubUrl,
            image: image);
    }

    public void Modify(string name, string heading, string githubUrl, string image)
    {
        Validate(
            ProjectId,
            name,
            heading,
            githubUrl,
            image);
        
        this.Name = name;
        this.Heading = heading;
        this.GithubUrl = githubUrl;
        this.Image = image;
    }
    
    
    private static void Validate(
        Guid projectId,
        string name,
        string heading,
        string githubUrl,
        string image)
    {
        if (projectId == Guid.Empty)
            throw new DomainException(
                "Project ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Contributor name cannot be empty.");

        if (string.IsNullOrWhiteSpace(heading))
            throw new DomainException(
                "Contributor heading cannot be empty.");

        if (string.IsNullOrWhiteSpace(githubUrl))
            throw new DomainException(
                "Contributor GitHub URL cannot be empty.");

        if (!Uri.TryCreate(
                githubUrl.Trim(),
                UriKind.Absolute,
                out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainException(
                "Contributor GitHub URL must be a valid HTTP or HTTPS URL.");
        }

        if (!string.Equals(
                uri.Host,
                "github.com",
                StringComparison.OrdinalIgnoreCase) &&
            !uri.Host.EndsWith(
                ".github.com",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException(
                "Contributor URL must be a GitHub URL.");
        }

        if (string.IsNullOrWhiteSpace(image))
            throw new DomainException(
                "Contributor image cannot be empty.");
    }
}
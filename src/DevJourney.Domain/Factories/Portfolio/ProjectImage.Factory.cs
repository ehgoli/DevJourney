using DevJourney.Domain.Exceptions;

namespace DevJourney.Domain.Entities.Portfolio;

public partial class ProjectImage
{
    private ProjectImage()
    {
    }

    private ProjectImage(Guid projectId, string fileName, string? caption, int displayOrder)
    {
        Validate(
            projectId,
            fileName,
            displayOrder);
        
        ProjectId = projectId;
        FileName = fileName;
        Caption = caption;
        DisplayOrder = displayOrder;
    }

    internal static ProjectImage Create(Guid projectId, string fileName, string? caption, int displayOrder)
    {
        return new ProjectImage(projectId: projectId,
            fileName: fileName,
            caption: caption,
            displayOrder: displayOrder);
    }
    
    public void Modify(string fileName, string? caption, int displayOrder)
    {
        Validate(
            ProjectId,
            fileName,
            displayOrder);
        
        FileName = fileName;
        Caption = caption;
        DisplayOrder = displayOrder;
    }
    
    
    private static void Validate(
        Guid projectId,
        string fileName,
        int displayOrder)
    {
        if (projectId == Guid.Empty)
            throw new DomainException(
                "Project ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new DomainException(
                "File name cannot be empty.");

        if (displayOrder < 0)
            throw new DomainException(
                "Display order cannot be negative.");
    }
}
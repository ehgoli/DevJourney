using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Portfolio;
using DevJourney.Infrastructure.Persistence.Context;

namespace DevJourney.Infrastructure.Persistence.Repositories.Portfolio;

public class ProjectRepository(AppDbContext context) : IProjectRepository
{
}
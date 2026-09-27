using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Infrastructure.Persistence.Context;

namespace DevJourney.Infrastructure.Persistence.Repositories.Identity;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
}
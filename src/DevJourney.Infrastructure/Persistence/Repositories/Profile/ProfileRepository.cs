using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Profile;
using DevJourney.Infrastructure.Persistence.Context;

namespace DevJourney.Infrastructure.Persistence.Repositories.Profile;

public class ProfileRepository(AppDbContext context) : IProfileRepository
{
}
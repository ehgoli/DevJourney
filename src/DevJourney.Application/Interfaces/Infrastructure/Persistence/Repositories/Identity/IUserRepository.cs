using DevJourney.Domain.Entities.Identity;

namespace DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;

public interface IUserRepository
{
    Task<User?> GetByPhoneAsync(string phone);
}
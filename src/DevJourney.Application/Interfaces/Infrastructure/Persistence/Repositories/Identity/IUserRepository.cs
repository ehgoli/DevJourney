using DevJourney.Domain.Entities.Identity;

namespace DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
 
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}
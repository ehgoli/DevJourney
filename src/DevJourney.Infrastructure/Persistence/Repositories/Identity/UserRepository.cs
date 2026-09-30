using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Domain.Entities.Identity;
using DevJourney.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevJourney.Infrastructure.Persistence.Repositories.Identity;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

    public async Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken)
    {
        return await context.Users.SingleOrDefaultAsync(x => x.Phone == phone, cancellationToken);
    }

    
    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        context.Attach(user).State = EntityState.Modified;
        return context.SaveChangesAsync(cancellationToken);
    }
}
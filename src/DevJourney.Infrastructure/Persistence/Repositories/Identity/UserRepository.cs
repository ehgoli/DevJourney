using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Identity;
using DevJourney.Domain.Entities.Identity;
using DevJourney.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevJourney.Infrastructure.Persistence.Repositories.Identity;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByPhoneAsync(string phone)
    {
        return await context.Users.SingleOrDefaultAsync(x => x.Phone == phone);
    }
}
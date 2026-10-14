
namespace DevJourney.Application.Interfaces.Infrastructure.Persistence.Transactions;

public interface IDatabaseTransaction
{
    Task BeginAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

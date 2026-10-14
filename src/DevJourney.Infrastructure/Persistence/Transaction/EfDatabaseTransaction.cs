using DevJourney.Application.Interfaces.Infrastructure.Persistence.Transactions;
using DevJourney.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevJourney.Infrastructure.Persistence.Transaction;

public sealed class EfDatabaseTransaction(
    AppDbContext dbContext) : IDatabaseTransaction
{
    private IDbContextTransaction? _transaction;

    public async Task BeginAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException(
                "A database transaction is already active.");

        _transaction = await dbContext.Database.BeginTransactionAsync(
            cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException(
                "No active database transaction exists.");

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException(
                "No active database transaction exists.");

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();

        _transaction = null;
    }
}
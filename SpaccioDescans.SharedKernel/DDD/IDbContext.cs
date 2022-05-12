using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpaccioDescans.SharedKernel.DDD;

public interface IDbContext
{
    DatabaseFacade DatabaseFacade { get; }

    IDbContextTransaction GetCurrentTransaction();

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
}
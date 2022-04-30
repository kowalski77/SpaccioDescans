using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : IStoreRepository
{
    private readonly SpaccioContext context;

    public StoreRepository(SpaccioContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    public async Task<Store> GetByCodeAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.context.Stores.FirstAsync(x => x.Code == id, cancellationToken: cancellationToken);
    }
}
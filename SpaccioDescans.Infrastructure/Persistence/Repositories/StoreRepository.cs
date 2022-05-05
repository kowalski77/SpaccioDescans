using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : IStoreRepository
{
    private readonly SpaccioContext context;

    public StoreRepository(SpaccioContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Store> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.context.Stores.FirstAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }
}
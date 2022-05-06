using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : IStoreRepository
{
    private readonly SpaccioContext context;
    private readonly ITenantProvider tenantProvider;

    public StoreRepository(SpaccioContext context, ITenantProvider tenantProvider)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
    }

    public async Task<Store> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.context.Stores.FirstAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Store> GetCurrentStore(CancellationToken cancellationToken = default)
    {
        var tenantId = this.tenantProvider.GetTenantId();

        return await this.context.Stores.FirstAsync(x => x.Id == tenantId, cancellationToken);
    }
}
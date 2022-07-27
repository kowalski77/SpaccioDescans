using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : Repository<Store>, IStoreRepository
{
    private readonly ITenantProvider tenantProvider;

    public StoreRepository(SpaccioContext context, ITenantProvider tenantProvider) 
        : base(context)
    {
        this.tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
    }

    public async Task<Store> GetCurrentStore(CancellationToken cancellationToken = default)
    {
        var tenantId = this.tenantProvider.GetTenantId();

        return await this.Context.Stores.FirstAsync(x => x.Id == tenantId, cancellationToken);
    }

    public override async Task<Store?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var store = await this.Context.Stores.FindAsync(new object?[] { id }, cancellationToken);
        if (store is null)
        {
            return null;
        }

        await this.Context.Entry(store).Collection(x => x.ProductStores).LoadAsync(cancellationToken);

        return store;
    }
}
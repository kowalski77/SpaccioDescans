using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Stores;
using SpaccioDescans.SharedKernel.DDD;

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

        return await this.Context.Set<Store>().FirstAsync(x => x.Id == tenantId, cancellationToken);
    }
}
namespace SpaccioDescans.Core.Domain.Stores;

public class StoreService : IStoreService
{
    private readonly IStoreCache tenantProvider;
    private readonly IStoreRepository storeRepository;

    public StoreService(IStoreCache tenantProvider, IStoreRepository storeRepository)
    {
        this.tenantProvider = tenantProvider;
        this.storeRepository = storeRepository;
    }

    public async Task<IReadOnlyList<StoreDto>> GetStoresAsync(CancellationToken cancellationToken = default)
    {
        var stores = await this.storeRepository.GetAllAsync(cancellationToken);

        return stores.Select(store => new StoreDto(store.Id, store.Name)).ToList();
    }

    public void SetUserStore(string userId, int storeId)
    {
        this.tenantProvider.SetUserStore(userId, storeId);
    }
}

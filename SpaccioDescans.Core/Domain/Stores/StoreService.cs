namespace SpaccioDescans.Core.Domain.Stores;

public class StoreService : IStoreService
{
    private readonly IStoreRepository storeRepository;
    private readonly IStoreCache storeCache;

    public StoreService(IStoreRepository storeRepository, IStoreCache storeCache)
    {
        this.storeRepository = storeRepository;
        this.storeCache = storeCache;
    }

    public async Task<Store> GetStoreByUserAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException($"'{nameof(userName)}' cannot be null or whitespace.", nameof(userName));
        }

        var storeId = storeCache.GetUserStore(userName);
        var store = await this.storeRepository.GetByIdAsync(storeId);

        return store!;
    }
}

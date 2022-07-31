namespace SpaccioDescans.Core.Domain.Stores;

public interface IStoreService
{
    Task<IReadOnlyList<StoreDto>> GetStoresAsync(CancellationToken cancellationToken = default);
    void SetUserStore(string userId, int storeId);
}

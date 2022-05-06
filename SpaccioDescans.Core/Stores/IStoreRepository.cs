namespace SpaccioDescans.Core.Stores;

public interface IStoreRepository
{
    Task<Store> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Store> GetCurrentStore(CancellationToken cancellationToken = default);
}
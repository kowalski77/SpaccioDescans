namespace SpaccioDescans.Core.Stores;

public interface IStoreRepository
{
    Task<Store> GetByCodeAsync(int id, CancellationToken cancellationToken = default);
}
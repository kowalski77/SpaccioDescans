namespace SpaccioDescans.Core.Stores;

public interface IStoreRepository
{
    Task<Store> GetByCodeAsync(int code, CancellationToken cancellationToken = default);
}
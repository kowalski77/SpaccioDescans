using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Stores;

public interface IStoreRepository : IRepository<Store>
{
    Task<Store> GetCurrentStore(CancellationToken cancellationToken = default);
}
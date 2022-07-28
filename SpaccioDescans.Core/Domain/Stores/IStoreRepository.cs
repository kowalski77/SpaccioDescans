using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Stores;

public interface IStoreRepository : IRepository<Store>
{
    Task<Store> GetCurrentStore(CancellationToken cancellationToken = default);
}
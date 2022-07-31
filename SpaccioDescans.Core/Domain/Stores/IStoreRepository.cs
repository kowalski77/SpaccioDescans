using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Stores;

public interface IStoreRepository : IRepository<Store>
{
    Task<IReadOnlyList<Store>> GetAllAsync(CancellationToken cancellationToken = default);
}
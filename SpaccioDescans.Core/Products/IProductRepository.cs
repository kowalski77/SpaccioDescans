using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Products;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> GetByIdWithStoresAsync(Guid id, CancellationToken cancellationToken = default);
}
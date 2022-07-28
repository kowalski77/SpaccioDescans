using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Products;

public interface IProductRepository : IRepository<Product>
{
    //Task<Product> GetByIdWithStoresAsync(long id, CancellationToken cancellationToken = default);
}
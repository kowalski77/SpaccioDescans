using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly SpaccioContext context;

    public ProductRepository(SpaccioContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public IUnitOfWork UnitOfWork => this.context;

    public Product Add(Product item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var product = this.context.Add(item);

        return product.Entity;
    }

    public async Task<Product> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.context.Products.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Product> GetByIdWithStoresAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.context.Products
            .Include(x => x.ProductStores)
            .ThenInclude(y => y.Store)
            .FirstAsync(x => x.Id == id, cancellationToken);
    }
}
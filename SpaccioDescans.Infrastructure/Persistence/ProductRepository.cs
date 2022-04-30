using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

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

    public async Task<Product?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.context.Products.FindAsync(new object? [] { id }, cancellationToken);
    }
}
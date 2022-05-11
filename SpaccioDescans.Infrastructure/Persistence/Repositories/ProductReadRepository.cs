using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class ProductReadRepository : IProductReadRepository
{
    private readonly SpaccioContext context;

    public ProductReadRepository(SpaccioContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .Select(x => new ProductDto(x.Id, x.Vendor, x.Name, x.Description, x.Measures, x.NetPrice.Value, x.ProductStores.Select(y =>
                new ProductStoreDto(y.Store.Id, y.Quantity))))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return products;
    }

    public async Task<IReadOnlyList<ProductOutOfStockDto>> GetAllOutOfStock(CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .Where(x => x.ProductStores.Any(y => y.Quantity < 0))
            .Select(x => new ProductOutOfStockDto(
                x.Id,
                $"{x.Name} - {x.Description} - {x.Measures}",
                x.OrderDetails.Select(y => y.Order.Id),
                x.ProductStores.Select(y => new ProductStoreDto(y.Store.Id, y.Quantity))))
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return products;
    }
}
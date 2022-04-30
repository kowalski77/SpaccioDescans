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
            .Select(x => new ProductDto(x.Id, x.Code, x.Vendor, x.Name, x.Description, x.Measures, x.NetPrice.Value, x.ProductStores.Select(y => 
                new ProductStoreDto(y.Store.Code, y.Quantity.Value))))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return products;
    }
}
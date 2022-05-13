using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(SpaccioContext context) : base(context)
    {
    }
    //GetById with EF AutoInclude
}
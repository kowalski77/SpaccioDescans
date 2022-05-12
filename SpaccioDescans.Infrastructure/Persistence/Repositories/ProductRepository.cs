using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(SpaccioContext context) : base(context)
    {
    }

    //GetById with EF AutoInclude
}
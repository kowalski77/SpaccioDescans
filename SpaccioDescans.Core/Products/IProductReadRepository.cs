namespace SpaccioDescans.Core.Products;

public interface IProductReadRepository
{
    Task<IReadOnlyList<ProductDto>> GetAll(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductOutOfStockDto>> GetAllOutOfStock(CancellationToken cancellationToken = default);
}
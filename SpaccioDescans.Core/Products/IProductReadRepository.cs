namespace SpaccioDescans.Core.Products;

public interface IProductReadRepository
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
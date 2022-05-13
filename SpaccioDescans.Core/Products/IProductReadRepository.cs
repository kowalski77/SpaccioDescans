namespace SpaccioDescans.Core.Products;

public interface IProductReadRepository
{
    Task<IReadOnlyList<ProductDto>> GetAll(CancellationToken cancellationToken = default);
}
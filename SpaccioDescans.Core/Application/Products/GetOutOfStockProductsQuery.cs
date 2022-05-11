using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application.Products;

public sealed record GetOutOfStockProductsQuery : IRequest<IReadOnlyList<ProductOutOfStockDto>>;

public sealed class GetOutOfStockProductsHandler : IRequestHandler<GetOutOfStockProductsQuery, IReadOnlyList<ProductOutOfStockDto>>
{
    private readonly IProductReadRepository productReadRepository;

    public GetOutOfStockProductsHandler(IProductReadRepository productReadRepository)
    {
        this.productReadRepository = productReadRepository ?? throw new ArgumentNullException(nameof(productReadRepository));
    }

    public async Task<IReadOnlyList<ProductOutOfStockDto>> Handle(GetOutOfStockProductsQuery request, CancellationToken cancellationToken)
    {
        return await this.productReadRepository.GetAllOutOfStock(cancellationToken);
    }
}
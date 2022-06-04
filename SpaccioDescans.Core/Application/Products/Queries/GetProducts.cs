using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application.Products.Queries;

public sealed record GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductReadRepository productReadRepository;

    public GetProductsHandler(IProductReadRepository productReadRepository)
    {
        this.productReadRepository = productReadRepository ?? throw new ArgumentNullException(nameof(productReadRepository));
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await this.productReadRepository.GetAllAsync(cancellationToken);
    }
}
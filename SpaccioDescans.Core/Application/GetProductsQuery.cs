using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application;

public sealed record GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductReadRepository productReadRepository;

    public GetProductsHandler(IProductReadRepository productReadRepository)
    {
        ArgumentNullException.ThrowIfNull(productReadRepository);
        this.productReadRepository = productReadRepository;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await this.productReadRepository.GetAll(cancellationToken).ConfigureAwait(true);
    }
}
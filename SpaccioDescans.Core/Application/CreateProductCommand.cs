using MediatR;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.Results;

namespace SpaccioDescans.Core.Application;

public sealed record CreateProductCommand(string Name, string Description, string Measures, decimal NetPrice) : IRequest<Result<Guid>>;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        ArgumentNullException.ThrowIfNull(productRepository);
        this.productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);

        var result = await Result.Init
            .Validate(netPrice)
            .OnSuccess(async () => await this.CreateProductAsync(request, netPrice.Value, cancellationToken).ConfigureAwait(true))
            .ConfigureAwait(true);

        return result.Value.Id;
    }

    private async Task<Result<Product>> CreateProductAsync(CreateProductCommand request, Price price, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Description, request.Measures, price);
        var newlyProduct = this.productRepository.Add(product);

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(true);

        return newlyProduct;
    }
}
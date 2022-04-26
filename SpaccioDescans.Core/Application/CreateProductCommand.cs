using MediatR;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.Results;

namespace SpaccioDescans.Core.Application;

public sealed record CreateProductCommand(Guid Id, string Name, string Description, string Measures, decimal NetPrice, int Quantity) : IRequest<Result<int>>;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IProductRepository productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        ArgumentNullException.ThrowIfNull(productRepository);
        this.productRepository = productRepository;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);
        var quantity = Quantity.CreateInstance(request.Quantity);

        var result = await Result.Init
            .Validate(netPrice, quantity)
            .OnSuccess(async () => await this.CreateProductAsync(request, netPrice.Value, quantity.Value, cancellationToken).ConfigureAwait(true))
            .ConfigureAwait(true);

        return result.Value.Code;
    }

    private async Task<Result<Product>> CreateProductAsync(CreateProductCommand request, Price price,  Quantity quantity, CancellationToken cancellationToken)
    {
        var product = new Product(request.Id, request.Name, request.Description, request.Measures, price, quantity);
        var newlyProduct = this.productRepository.Add(product);

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(true);

        return newlyProduct;
    }
}
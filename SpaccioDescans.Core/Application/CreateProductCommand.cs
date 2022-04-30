using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application;

public sealed record CreateProductCommand(Guid Id, string Vendor, string Name, string Description, string Measures, decimal NetPrice, int Quantity) : IRequest<int>;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);
        var quantity = Quantity.CreateInstance(request.Quantity);

        var product = new Product(request.Id, request.Vendor, request.Name, request.Description, request.Measures, netPrice, quantity);
        var newlyProduct = this.productRepository.Add(product);

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return newlyProduct.Code;
    }
}
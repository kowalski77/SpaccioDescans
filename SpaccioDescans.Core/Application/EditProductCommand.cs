using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application;

public sealed record EditProductCommand(Guid Id, string Vendor, string Name, string Description, string Measures, decimal NetPrice, int Quantity) : IRequest;

public sealed class EditProductHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductRepository productRepository;

    public EditProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Unit> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);
        var quantity = Quantity.CreateInstance(request.Quantity);

        var product = await this.productRepository.GetAsync(request.Id, cancellationToken);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id: {request.Id} not found");
        }

        product.NetPrice = netPrice;
        product.Quantity = quantity;
        product.Name = request.Name;
        product.Description = request.Description;
        product.Measures = request.Measures;
        product.Vendor = request.Vendor;

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}
using MediatR;
using SpaccioDescans.Core.Products;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Core.Application;

public sealed record CreateProductCommand(Guid Id, string Vendor, string Name, string Description, string Measures, decimal NetPrice, IEnumerable<StoreQuantity> StoreQuantities) : IRequest<int>;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository productRepository;
    private readonly IStoreRepository storeRepository;

    public CreateProductHandler(IProductRepository productRepository, IStoreRepository storeRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        this.storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);
        var product = new Product(request.Id, request.Vendor, request.Name, request.Description, request.Measures, netPrice);

        foreach (var storeQuantity in request.StoreQuantities)
        {
            var store = await this.storeRepository.GetByCodeAsync(storeQuantity.StoreCode, cancellationToken);
            var quantity = Quantity.CreateInstance(storeQuantity.Quantity);

            product.AddToStore(store, quantity);
        }

        var newlyProduct = this.productRepository.Add(product);
        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return newlyProduct.Code;
    }
}
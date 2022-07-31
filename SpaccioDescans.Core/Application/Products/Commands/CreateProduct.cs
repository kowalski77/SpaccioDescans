using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Application.Products.Commands;

public sealed record CreateProductCommand(string Vendor, string Name, string Description, string Measures, decimal NetPrice, IEnumerable<StoreQuantity> StoreQuantities) : ICommand<long>;

public sealed class CreateProductHandler : ICommandHandler<CreateProductCommand, long>
{
    private readonly IProductRepository productRepository;
    private readonly IStoreRepository storeRepository;

    public CreateProductHandler(IProductRepository productRepository, IStoreRepository storeRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        this.storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
    }

    public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);
        var product = new Product(request.Vendor, request.Name, request.Description, request.Measures, netPrice);

        foreach (var storeQuantity in request.StoreQuantities)
        {
            var store = await this.storeRepository.GetByIdAsync(storeQuantity.StoreCode, cancellationToken);
            var quantity = Quantity.CreateInstance(storeQuantity.Quantity);

            product.AddToStore(store!, quantity);
        }

        var newlyProduct = this.productRepository.Save(product);
        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return newlyProduct.Id;
    }
}
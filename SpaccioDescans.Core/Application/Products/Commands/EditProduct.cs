using MediatR;
using SpaccioDescans.Core.Products;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Core.Application.Products.Commands;

public sealed record EditProductCommand(long Id, string Vendor, string Name, string Description, string Measures, decimal NetPrice, IEnumerable<StoreQuantity> StoreQuantities) : IRequest;

public sealed class EditProductHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductRepository productRepository;
    private readonly IStoreRepository storeRepository;

    public EditProductHandler(IProductRepository productRepository, IStoreRepository storeRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        this.storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
    }

    public async Task<Unit> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var netPrice = Price.CreateInstance(request.NetPrice);
        var product = await this.productRepository.GetAsync(request.Id, cancellationToken);

        product.NetPrice = netPrice;
        product.Name = request.Name;
        product.Description = request.Description;
        product.Measures = request.Measures;
        product.Vendor = request.Vendor;

        foreach (var storeQuantity in request.StoreQuantities)
        {
            var store = await this.storeRepository.GetAsync(storeQuantity.StoreCode, cancellationToken);
            var quantity = Quantity.CreateInstance(storeQuantity.Quantity);

            product.EditInStore(store, quantity);
        }

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}
using MediatR;
using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Products.Commands;

public sealed record EditProductCommand(long Id, string Vendor, string Name, string Description, string Measures, decimal NetPrice, IEnumerable<StoreQuantity> StoreQuantities) : ICommand<Unit>;

public sealed class EditProductHandler : ICommandHandler<EditProductCommand, Unit>
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

        var product = await this.productRepository.GetByIdAsync(request.Id, cancellationToken);

        var netPrice = Price.CreateInstance(request.NetPrice);

        product!.Edit(netPrice);
        product.Edit(request.Vendor, request.Name, request.Description, request.Measures);

        foreach (var storeQuantity in request.StoreQuantities)
        {
            var store = await this.storeRepository.GetByIdAsync(storeQuantity.StoreCode, cancellationToken);
            var quantity = Quantity.CreateInstance(storeQuantity.Quantity);

            product.EditQuantityInStore(store!, quantity);
        }

        await this.productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}
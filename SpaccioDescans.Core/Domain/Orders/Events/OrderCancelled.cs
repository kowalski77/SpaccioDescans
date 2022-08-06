using MediatR;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Core.Domain.Orders.Events;

public sealed record OrderCancelled(IEnumerable<ProductQuantity> ProductQuantities, long StoreId) : INotification;

public class OrderCancelledHandler : INotificationHandler<OrderCancelled>
{
    private readonly IStoreRepository storeRepository;

    public OrderCancelledHandler(IStoreRepository storeRepository)
    {
        this.storeRepository = storeRepository;
    }

    public async Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var store = await this.storeRepository.GetByIdAsync(notification.StoreId, cancellationToken);

        foreach (var productQuantity in notification.ProductQuantities)
        {
            var productStore = store!.ProductStores.First(x => x.ProductId == productQuantity.ProductId);
            productStore.Quantity += productQuantity.Quantity;
        }
    }
}

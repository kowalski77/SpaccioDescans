using MediatR;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Core.Application.Orders.Events;

public sealed record OrderCancelled(IEnumerable<long> ProductIds, int StoreId) : INotification;

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

        foreach (var productId in notification.ProductIds)
        {
            var productStore = store!.ProductStores.First(x => x.ProductId == productId);
            productStore.Quantity++;
        }
    }
}

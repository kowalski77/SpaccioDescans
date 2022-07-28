using MediatR;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Core.Application.Orders.Events;

public sealed record OrderCreated(IEnumerable<long> ProductIds, int StoreId) : INotification;

public class OrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IStoreRepository storeRepository;

    public OrderCreatedHandler(IStoreRepository storeRepository)
    {
        this.storeRepository = storeRepository;
    }

    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var store = await this.storeRepository.GetAsync(notification.StoreId, cancellationToken);

        foreach (var productId in notification.ProductIds)
        {
            var productStore = store!.ProductStores.First(x => x.ProductId == productId);
            productStore.Quantity--;
        }
    }
}
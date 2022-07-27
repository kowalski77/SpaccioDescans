using MediatR;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Core.Application.Orders.Events;

public sealed record OrderCancelled(IEnumerable<long> ProductIds, int StoreId) : INotification;

public class OrderCancelledHandler : INotificationHandler<OrderCancelled>
{
    private readonly IProductRepository productRepository;

    public OrderCancelledHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        foreach (var productId in notification.ProductIds)
        {
            var product = await this.productRepository.GetAsync(productId, cancellationToken);
            var store = product!.ProductStores.First(x => x.StoreId == notification.StoreId);
            store.Quantity++;
        }
    }
}

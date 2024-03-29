﻿using MediatR;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Core.Domain.Orders.Events;

public sealed record OrderCreated(IEnumerable<ProductQuantity> ProductQuantities, long StoreId) : INotification;

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

        var store = await this.storeRepository.GetByIdAsync(notification.StoreId, cancellationToken);

        foreach (var productQuantity in notification.ProductQuantities)
        {
            var productStore = store!.ProductStores.First(x => x.ProductId == productQuantity.ProductId);
            productStore.Quantity -= productQuantity.Quantity;
        }
    }
}
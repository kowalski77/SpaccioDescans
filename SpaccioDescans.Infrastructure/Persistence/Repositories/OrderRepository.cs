﻿using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Orders;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly SpaccioContext context;

    public OrderRepository(SpaccioContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUnitOfWork UnitOfWork => this.context;

    public Order Add(Order item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var orderEntry = this.context.Orders.Add(item);

        return orderEntry.Entity;
    }

    public async Task<Order> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.context.Orders.FirstAsync(x => x.Id == id, cancellationToken);
    }
}
using SpaccioDescans.Core.Orders;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(SpaccioContext context) : base(context)
    {
    }

    public override async Task<Order> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var order = await this.Context.Set<Order>().FindAsync(new object?[] { id }, cancellationToken) ??
                    throw new InvalidOperationException($"Entity type: {nameof(Order)} with id: {id} not found");

        await this.Context.Entry(order).Reference(x=>x.Customer).LoadAsync(cancellationToken);
        await this.Context.Entry(order).Collection(x=>x.Payments).LoadAsync(cancellationToken);
        await this.Context.Entry(order).Collection(x=>x.OrderDetails).LoadAsync(cancellationToken);

        return order;
    }
}
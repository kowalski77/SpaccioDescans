using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Domain.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(SpaccioContext context) : base(context)
    {
    }

    public override async Task<Order?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var order = await this.Context.Orders.FindAsync(new object?[] { id }, cancellationToken);
        if(order is null)
        {
            return null;
        }

        await this.Context.Entry(order).Reference(x => x.Customer).LoadAsync(cancellationToken);
        await this.Context.Entry(order).Collection(x => x.Payments).LoadAsync(cancellationToken);
        await this.Context.Entry(order).Collection(x => x.OrderDetails).LoadAsync(cancellationToken);
        await this.Context.Entry(order).Collection(x => x.OrderDetails).Query().Include(x => x.Product).LoadAsync(cancellationToken);

        return order;
    }
}
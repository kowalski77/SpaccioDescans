using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class OrderReadRepository : IOrderReadRepository
{
    private readonly SpaccioContext context;

    public OrderReadRepository(SpaccioContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await this.context.Orders
            .Select(x => new OrderDto(
                x.Id,
                x.Customer.Name,
                x.Date,
                x.Store.Name,
                x.Status,
                x.Total))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return orders;
    }
}
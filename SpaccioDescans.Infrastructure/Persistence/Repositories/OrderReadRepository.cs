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

    public async Task<OrderDetailDto> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var order = await this.context.Orders.FindAsync(new object?[] { id }, cancellationToken) ??
                    throw new InvalidOperationException($"Entity type: {nameof(Order)} with id: {id} not found");

        await this.context.Entry(order).Reference(x => x.Customer).LoadAsync(cancellationToken);
        await this.context.Entry(order).Collection(x => x.Payments).LoadAsync(cancellationToken);
        await this.context.Entry(order).Collection(x => x.OrderDetails).LoadAsync(cancellationToken);

        var orderDto = new OrderDetailDto(
            order.Id,
            new CustomerDto(
                order.Customer.Name,
                order.Customer.Address,
                order.Customer.Nif,
                order.Customer.Phone));

        return orderDto;
    }
}
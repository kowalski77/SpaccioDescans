namespace SpaccioDescans.Core.Orders;

public interface IOrderReadRepository
{
    Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
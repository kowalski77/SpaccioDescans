namespace SpaccioDescans.Core.Orders;

public interface IOrderReadRepository
{
    Task<OrderDetailDto> GetAsync(long id, CancellationToken cancellationToken = default);
}
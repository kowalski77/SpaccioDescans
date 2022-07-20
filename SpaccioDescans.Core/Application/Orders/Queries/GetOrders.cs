using MediatR;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Core.Application.Orders.Queries;

public sealed record GetOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderDto>>
{
    private readonly IOrderReadRepository orderReadRepository;

    public GetOrdersHandler(IOrderReadRepository orderReadRepository)
    {
        this.orderReadRepository = orderReadRepository;
    }

    public async Task<IReadOnlyList<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await this.orderReadRepository.GetAllAsync(cancellationToken);
    }
}

using MediatR;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Application.Orders.Commands;

public sealed record CancelOrderCommand(long Id) : ICommand<Unit>;

public sealed class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, Unit>
{
    private readonly IOrderRepository orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = await this.orderRepository.GetByIdAsync(request.Id, cancellationToken);
        order!.Cancel();

        await this.orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}

using MediatR;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Application.Orders.Commands;

public sealed record EditPaymentCommand(long OrderId, decimal Cash, decimal CreditCard, decimal Financed) : ICommand<Unit>;

public sealed record EditPaymentCommandHandler : ICommandHandler<EditPaymentCommand, Unit>
{
    private readonly IOrderRepository orderRepository;

    public EditPaymentCommandHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(EditPaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = await this.orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        var cash = new Payment(request.Cash, PaymentMethod.Cash);
        var creditCard = new Payment(request.CreditCard, PaymentMethod.CreditCard);
        var financed = new Payment(request.Financed, PaymentMethod.Financed);

        order!.EditPayments(cash, creditCard, financed);

        await this.orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}

using MediatR;
using SpaccioDescans.Core.Orders;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Application.Orders.Commands;

public sealed record EditCustomerInfoCommand(long OrderId, string Name, string Address, string Nif, string Phone) : ICommand<Unit>;

public class EditCustomerInfoHandler : ICommandHandler<EditCustomerInfoCommand, Unit>
{
    private readonly IOrderRepository orderRepository;

    public EditCustomerInfoHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(EditCustomerInfoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = await this.orderRepository.GetAsync(request.OrderId, cancellationToken);
        
        var customer = new Customer(
            request.Name, 
            request.Address, 
            request.Nif, 
            request.Phone);
        
        order!.EditCustomer(customer);

        await this.orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}

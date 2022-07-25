﻿using MediatR;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Core.Application.Orders.Commands;

public sealed record EditCustomerInfoCommand(long OrderId, string Name, string Address, string Nif, string Phone) : IRequest;

public class EditCustomerInfoHandler : IRequestHandler<EditCustomerInfoCommand, Unit>
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
        if(order is null)
        {
            throw new InvalidOperationException($"Order with id {request.OrderId} does not exist");
        }
        
        var customer = new Customer(
            request.Name, 
            request.Address, 
            request.Nif, 
            request.Phone);
        
        order.EditCustomer(customer);

        await this.orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}
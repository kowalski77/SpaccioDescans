using MediatR;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Core.Application.Orders;

public sealed record CreateOrderCommand(int StoreCode, CustomerInfo CustomerInfo, 
    IEnumerable<OrderDetailItem> OrderDetailItems, IEnumerable<PaymentData> PaymentDatas) : IRequest<long>;

public sealed record CustomerInfo(string Name, string Address, string City, string Phone);

public sealed record OrderDetailItem(int ProductCode, int Quantity, decimal Discount);

public sealed record PaymentData(PaymentMethod PaymentMethod, decimal Amount);

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly IOrderRepository orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = new Customer(request.CustomerInfo.Name, request.CustomerInfo.Address, 
            request.CustomerInfo.City, request.CustomerInfo.Phone);

//        IEnumerable<OrderDetail> orderDetails = request.OrderDetailItems.Select(x => new OrderDetail())


        //var order = new Order(store, customer, )

        //await this.orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        //return order.OrderId;

        throw new NotImplementedException();
    }
}
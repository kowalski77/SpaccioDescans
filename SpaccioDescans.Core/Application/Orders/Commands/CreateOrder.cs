using MediatR;
using SpaccioDescans.Core.Orders;
using SpaccioDescans.Core.Products;
using SpaccioDescans.Core.Stores;

namespace SpaccioDescans.Core.Application.Orders.Commands;

public sealed record CreateOrderCommand(CustomerInfo CustomerInfo, IEnumerable<OrderDetailItem> OrderDetailItems, IEnumerable<PaymentData> PaymentDataCollection) : IRequest<long>;

public sealed record CustomerInfo(string Name, string Address, string Nif, string Phone);

public sealed record OrderDetailItem(long ProductId, long StoreId, int Quantity, decimal Discount);

public sealed record PaymentData(PaymentMethod PaymentMethod, decimal Amount);

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IStoreRepository storeRepository;

    public CreateOrderHandler(IOrderRepository orderRepository, IProductRepository productRepository, IStoreRepository storeRepository)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        this.storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var store = await this.storeRepository.GetCurrentStore(cancellationToken);
        var customer = new Customer(request.CustomerInfo.Name, request.CustomerInfo.Address, request.CustomerInfo.Nif, request.CustomerInfo.Phone);
        var orderDetails = await this.GetOrderDetails(request.OrderDetailItems);
        var payments = request.PaymentDataCollection.Select(paymentData => new Payment(paymentData.Amount, paymentData.PaymentMethod));

        var order = new Order(store, customer, orderDetails, payments);

        _ = this.orderRepository.Save(order);
        await this.orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return order.Id;
    }

    private async Task<List<OrderDetail>> GetOrderDetails(IEnumerable<OrderDetailItem> orderDetailItems)
    {
        var orderDetailsCollection = new List<OrderDetail>();
        foreach (var orderDetailItem in orderDetailItems)
        {
            var product = await this.productRepository.GetAsync(orderDetailItem.ProductId);
            var orderDetail = new OrderDetail(product, OrderQuantity.CreateInstance(orderDetailItem.Quantity), Discount.CreateInstance(orderDetailItem.Discount));
            orderDetailsCollection.Add(orderDetail);
        }

        return orderDetailsCollection;
    }
}
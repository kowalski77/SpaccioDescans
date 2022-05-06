#pragma warning disable 8618
using SpaccioDescans.Core.Stores;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public sealed class Order : Entity, IAggregateRoot
{
    private readonly List<OrderDetail> orderDetails = new();
    private readonly List<Payment> payments = new();

    private Order() { }

    public Order(Store store, Customer customer, 
        IEnumerable<OrderDetail> orderDetailList, IEnumerable<Payment> paymentList)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(orderDetailList);
        ArgumentNullException.ThrowIfNull(paymentList);

        this.Store = store;
        this.Customer = customer;
        this.Date = DateTime.Now;
        this.orderDetails = orderDetailList.ToList();
        this.payments = paymentList.ToList();

        this.CalculateTotals();
        this.SetStatus();
    }

    public long Id { get; private set; }

    public DateTime Date { get; private set; }

    public Store Store { get; private set; }

    public Customer Customer { get; private set; }

    public OrderStatus Status { get; private set; }

    public string? Remarks { get; private set; }

    public IReadOnlyList<OrderDetail> OrderDetails => this.orderDetails;

    public IReadOnlyList<Payment> Payment => this.payments;

    public decimal SubTotal { get; private set; }

    public decimal Total { get; private set; }

    private void CalculateTotals()
    {
        this.SubTotal = this.orderDetails.Sum(x => x.SubTotal);
        this.Total = this.SubTotal + (this.SubTotal * SpaccioConstants.Vat / 100);
    }

    private void SetStatus()
    {
        var isTotallyPaid = this.payments.Sum(x => x.Amount) == this.Total;
        this.Status = isTotallyPaid ? OrderStatus.Completed : OrderStatus.Pending;
    }
}
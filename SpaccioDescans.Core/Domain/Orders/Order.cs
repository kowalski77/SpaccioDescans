#pragma warning disable 8618
using SpaccioDescans.Core.Domain.Orders.Events;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Domain.Orders;

public sealed class Order : Entity, IAggregateRoot
{
    private readonly List<OrderDetail> orderDetails = new();
    private readonly List<Payment> payments = new();

    private Order() { }

    public Order(Store store, Customer customer,
        IEnumerable<OrderDetail> orderDetailList, IEnumerable<Payment> paymentList)
    {
        ArgumentNullException.ThrowIfNull(orderDetailList);
        ArgumentNullException.ThrowIfNull(paymentList);

        this.Store = store ?? throw new ArgumentNullException(nameof(store));
        this.Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        this.Date = DateTime.Now;
        this.orderDetails = orderDetailList.ToList();
        this.payments = paymentList.ToList();

        this.CalculateTotals();
        this.SetStatus();
        this.AddDomainEvent(new OrderCreated(this.GetProductQuantities(), store.Id));
    }

    public long Id { get; private set; }

    public DateTime Date { get; private set; }

    public Store Store { get; private set; }

    public Customer Customer { get; private set; }

    public OrderStatus Status { get; private set; }

    public string? Remarks { get; private set; }

    public IReadOnlyList<OrderDetail> OrderDetails => this.orderDetails;

    public IReadOnlyList<Payment> Payments => this.payments;

    public decimal SubTotal { get; private set; }

    public decimal Total { get; private set; }

    public decimal Pending { get; private set; }

    private decimal AmountPaid => this.payments.Sum(x => x.Amount);

    private void CalculateTotals()
    {
        this.SubTotal = this.orderDetails.Sum(x => x.Total);
        this.Total = this.SubTotal + (this.SubTotal * SpaccioConstants.Vat / 100);
        this.Pending = this.Total - this.AmountPaid;
    }

    private void SetStatus()
    {
        this.Status = this.AmountPaid == this.Total ? OrderStatus.Completed : OrderStatus.Pending;
    }

    public void EditCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        this.Customer.Edit(customer.Name, customer.Address, customer.Nif, customer.Phone);
    }

    public void EditPayments(params Payment[] payments)
    {
        ArgumentNullException.ThrowIfNull(payments);

        this.payments.Clear();
        this.payments.AddRange(payments);
        this.SetStatus();
    }

    public void Cancel()
    {
        this.SoftDeleted = true;
        this.Status = OrderStatus.Cancelled;
        this.AddDomainEvent(new OrderCancelled(this.GetProductQuantities(), this.Store.Id));
    }

    private List<ProductQuantity> GetProductQuantities()
    {
        return this.orderDetails.Select(x => new ProductQuantity(x.Product.Id, x.Quantity.Value)).ToList();
    }
}
﻿#pragma warning disable 8618
using SpaccioDescans.Core.Stores;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Core.Orders;

public sealed class Order : Entity, IAggregateRoot
{
    private readonly List<OrderDetail> orderDetails = new();

    private Order() { }

    public Order(Store store, Customer customer, string remarks)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(customer);

        this.Store = store;
        this.Customer = customer;
        this.Status = OrderStatus.Pending;
        this.Remarks = remarks;
        this.Date = DateTime.Now;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public long OrderId { get; private set; }

    public DateTime Date { get; private set; }

    public Store Store { get; private set; }

    public Customer Customer { get; private set; }

    public OrderStatus Status { get; private set; }

    public string Remarks { get; private set; }

    public IReadOnlyList<OrderDetail> OrderDetails => this.orderDetails;

    public decimal SubTotal { get; private set; }

    public decimal Total { get; private set; }

    public void AddOrderDetail(OrderDetail orderDetail)
    {
        ArgumentNullException.ThrowIfNull(orderDetail);
        this.orderDetails.Add(orderDetail);
    }

    public void CalculateTotals()
    {
        this.SubTotal = this.orderDetails.Sum(x => x.SubTotal);
        this.Total = this.SubTotal + (this.SubTotal * SpaccioConstants.Vat / 100);
    }
}
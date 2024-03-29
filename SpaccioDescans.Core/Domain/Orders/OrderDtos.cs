﻿namespace SpaccioDescans.Core.Domain.Orders;

public class OrderSummaryDto
{
    public long Id { get; init; }

    public string Customer { get; init; } = default!;

    public DateTime CreatedAt { get; init; }

    public string Store { get; init; } = default!;

    public OrderStatus OrderStatus { get; init; }

    public decimal Total { get; init; }
}

public class OrderDto
{
    public long Id { get; init; }

    public decimal SubTotal { get; set; }

    public OrderStatus OrderStatus { get; init; }

    public CustomerDto Customer { get; internal set; } = default!;

    public ICollection<PaymentDto> Payments { get; internal set; } = new List<PaymentDto>();

    public ICollection<OrderDetailDto> OrderDetails { get; internal set; } = new List<OrderDetailDto>();
}

public class CustomerDto
{
    public string Name { get; init; } = default!;

    public string Address { get; init; } = default!;

    public string Nif { get; init; } = default!;

    public string Phone { get; init; } = default!;
}

public class PaymentDto
{
    public decimal Amount { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }
}

public class OrderDetailDto
{
    public int Quantity { get; set; }

    public decimal Discount { get; init; }

    public long ProductId { get; init; }

    public string ProductName { get; init; } = default!;

    public decimal NetPrice { get; init; }
}
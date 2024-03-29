﻿using System.Data.SqlClient;
using Dapper;
using MediatR;
using SpaccioDescans.Core.Domain.Orders;

namespace SpaccioDescans.Core.Domain.Orders.Queries;

public sealed record GetOrderByIdQuery(long Id) : IRequest<OrderDto>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly QuerySettings querySettings;

    public GetOrderByIdQueryHandler(QuerySettings querySettings)
    {
        this.querySettings = querySettings;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var connection = new SqlConnection(this.querySettings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var query = $@"select o.Id, o.SubTotal, o.Status as OrderStatus,
                        c.Name, c.Address, c.Nif, c.Phone, 
                        p.Amount, p.PaymentMethod, 
                        od.Quantity, od.Discount, pr.Id as ProductId, CONCAT(pr.Vendor, ' ', pr.Name, ' ', pr.Measures) as ProductName, pr.NetPrice
                        from Orders o 
						inner join Customer c on c.Id = o.CustomerId 
						inner join Payment p on p.OrderId = o.Id
						inner join OrderDetail od on od.OrderId = o.Id
						inner join Products pr on pr.Id = od.ProductId
                        where o.Id = {request.Id}";

        var orders = await connection.QueryAsync<OrderDto, CustomerDto, PaymentDto, OrderDetailDto, OrderDto>
            (query, (order, customer, payment, orderDetail) =>
        {
            order.Customer = customer;
            order.Payments.Add(payment);
            order.OrderDetails.Add(orderDetail);
            return order;
        }, splitOn: "Name, Amount, Quantity");

        var order = orders.First();

        order.Payments = orders.SelectMany(x => x.Payments).DistinctBy(x => x.PaymentMethod).ToList();
        order.OrderDetails = orders.SelectMany(x => x.OrderDetails).DistinctBy(x => x.ProductId).ToList();

        return order;
    }
}
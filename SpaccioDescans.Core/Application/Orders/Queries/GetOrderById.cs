using System.Data.SqlClient;
using Dapper;
using MediatR;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Core.Application.Orders.Queries;

public sealed record GetOrderByIdQuery(long Id) : IRequest<OrderEditDto>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderEditDto>
{
    private readonly QuerySettings querySettings;

    public GetOrderByIdQueryHandler(QuerySettings querySettings)
    {
        this.querySettings = querySettings;
    }

    public async Task<OrderEditDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var connection = new SqlConnection(this.querySettings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var query = $@"select o.Id, c.Name, c.Address, c.Nif, c.Phone, p.Amount, p.PaymentMethod
                        from Orders o 
						inner join Customer c on c.Id = o.CustomerId 
						inner join Payment p on p.OrderId = o.Id
                        where o.Id = {request.Id}";

        var orders = await connection.QueryAsync<OrderEditDto, CustomerEditDto, PaymentEditDto, OrderEditDto>
            (query, (order, customer, payment) =>
        {
            order.Customer = customer;
            order.Payments.Add(payment);
            return order;
        }, splitOn: "Name, Amount");

        var result = orders.GroupBy(o => o.Id).Select(g => 
        {
            var order = g.First();
            order.Payments = g.SelectMany(o => o.Payments).ToList();
            
            return order;
        }).First();

        return result;
    }
}
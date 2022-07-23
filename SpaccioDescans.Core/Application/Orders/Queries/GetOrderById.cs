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

        var query = $@"select o.Id, c.Name, c.Address, c.Nif, c.Phone 
                        from Orders o inner join Customer c
                        on o.CustomerId = c.Id
                        where o.Id = {request.Id}";

        var order = await connection.QueryAsync<OrderEditDto, CustomerEditDto, OrderEditDto>(query, (order, customer) =>
        {
            order.Customer = customer;
            return order;
        }, splitOn: "Name");

        return order.First();
    }
}
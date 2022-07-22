using System.Data.SqlClient;
using Dapper;
using MediatR;
using SpaccioDescans.Core.Orders;

namespace SpaccioDescans.Core.Application.Orders.Queries;

public sealed record GetOrdersQuery : IRequest<IEnumerable<OrderDto>>;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly QuerySettings querySettings;

    public GetOrdersHandler(QuerySettings querySettings)
    {
        this.querySettings = querySettings;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var connection = new SqlConnection(this.querySettings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var query = @"select o.Id, c.Name as Customer, o.Date as CreatedAt, s.Name as Store, o.Status as OrderStatus, o.Total 
                            from Orders o inner join Customer c
                            on o.CustomerId = c.Id
                            inner join Stores s
                            on o.StoreId = s.Id";

        var orders = await connection.QueryAsync<OrderDto>(query);

        return orders;
    }
}

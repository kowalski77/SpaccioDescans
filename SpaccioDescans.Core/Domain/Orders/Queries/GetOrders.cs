using System.Data.SqlClient;
using Dapper;
using MediatR;
using SpaccioDescans.Core.Domain.Orders;

namespace SpaccioDescans.Core.Domain.Orders.Queries;

public sealed record GetOrdersQuery : IRequest<IEnumerable<OrderSummaryDto>>;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderSummaryDto>>
{
    private readonly QuerySettings querySettings;

    public GetOrdersHandler(QuerySettings querySettings)
    {
        this.querySettings = querySettings;
    }

    public async Task<IEnumerable<OrderSummaryDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var connection = new SqlConnection(this.querySettings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var query = @"select o.Id, c.Name as Customer, o.Date as CreatedAt, s.Name as Store, o.Status as OrderStatus, o.Total 
                            from Orders o inner join Customer c
                            on o.CustomerId = c.Id
                            inner join Stores s
                            on o.StoreId = s.Id";

        var orders = await connection.QueryAsync<OrderSummaryDto>(query);

        return orders;
    }
}

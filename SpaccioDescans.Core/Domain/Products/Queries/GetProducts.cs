using System.Data.SqlClient;
using Dapper;
using MediatR;
using SpaccioDescans.Core.Domain.Products;

namespace SpaccioDescans.Core.Domain.Products.Queries;

public sealed record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly QuerySettings querySettings;

    public GetProductsHandler(QuerySettings querySettings)
    {
        this.querySettings = querySettings;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var connection = new SqlConnection(this.querySettings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var query = @"select p.Id, p.Vendor, p.Name, p.Description, p.Measures, p.NetPrice, ps.StoreId, ps.Quantity 
                            from Products p inner join ProductStore ps 
                            on p.Id = ps.ProductId";

        var products = await connection.QueryAsync<ProductDto, ProductStoreDto, ProductDto>(
            query,
            (product, productStore) =>
            {
                product.ProductStores.Add(productStore);
                return product;
            },
            splitOn: "StoreId");

        var result = products.GroupBy(x => x.Id).Select(g =>
        {
            var product = g.First();
            product.ProductStores = g.Select(x => x.ProductStores.First()).ToList();

            return product;
        });

        return result;
    }
}
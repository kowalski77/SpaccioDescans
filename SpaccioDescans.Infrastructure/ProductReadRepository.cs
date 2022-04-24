﻿using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Products;

namespace SpaccioDescans.Infrastructure;

public sealed class ProductReadRepository : IProductReadRepository
{
    private readonly SpaccioContext context;

    public ProductReadRepository(SpaccioContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var products = await this.context.Products
            .Select(x => new ProductDto(x.Id, x.Name, x.Description, x.Measures, x.NetPrice.Value))
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(true);

        return products;
    }
}
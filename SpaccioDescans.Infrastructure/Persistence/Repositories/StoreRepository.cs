﻿using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Domain.Stores;

namespace SpaccioDescans.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : Repository<Store>, IStoreRepository
{
    public StoreRepository(SpaccioContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Store>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Stores.ToListAsync(cancellationToken);
    }

    public override async Task<Store?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var store = await this.Context.Stores.FindAsync(new object?[] { id }, cancellationToken);
        if (store is null)
        {
            return null;
        }

        await this.Context.Entry(store).Collection(x => x.ProductStores).LoadAsync(cancellationToken);

        return store;
    }
}
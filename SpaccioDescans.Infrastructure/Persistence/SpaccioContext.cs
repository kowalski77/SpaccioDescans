﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : IdentityDbContext<IdentityUser>, IUnitOfWork
{
    private readonly ITenantProvider tenantProvider;

    public SpaccioContext(DbContextOptions options, ITenantProvider tenantProvider)
        : base(options)
    {
        this.tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
    }

    public DbSet<Product> Products { get; set; } = default!;

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = this.tenantProvider.GetTenantId();
        foreach (var entry in this.ChangeTracker.Entries<Entity>())
        {
            entry.Entity.TenantId = entry.State switch
            {
                EntityState.Added => tenantId,
                _ => entry.Entity.TenantId
            };
        }

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        var tenantId = this.tenantProvider.GetTenantId();
        builder.Entity<Product>().HasQueryFilter(x => x.TenantId == tenantId);

        base.OnModelCreating(builder);
    }
}
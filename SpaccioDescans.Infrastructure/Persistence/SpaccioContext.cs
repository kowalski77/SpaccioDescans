using MediatR;
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Orders;
using SpaccioDescans.Core.Products;
using SpaccioDescans.Core.Stores;
using SpaccioDescans.Infrastructure.Persistence.Configurations;
using SpaccioDescans.Infrastructure.Transactions;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : TransactionContext
{
    public SpaccioContext(DbContextOptions options, IMediator mediator)
        : base(options, mediator)
    {
    }

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<Store> Stores { get; set; } = default!;

    public DbSet<Order> Orders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        base.OnModelCreating(builder);
    }
}
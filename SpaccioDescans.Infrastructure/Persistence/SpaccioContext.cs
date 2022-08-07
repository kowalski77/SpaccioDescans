using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Infrastructure.Persistence.Configurations;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : IdentityDbContext<IdentityUser>, IUnitOfWork
{
    private readonly IMediator mediator;

    public SpaccioContext(
        DbContextOptions options,
        IMediator mediator)
        : base(options)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<Order> Orders { get; set; } = default!;

    public DbSet<Store> Stores { get; set; } = default!;


    public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await this.mediator.PublishDomainEventsAsync(this, cancellationToken).ConfigureAwait(false);

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        base.OnModelCreating(builder);
    }
}
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : DbContext, IUnitOfWork
{
    private readonly ITenantService tenantService;

    public SpaccioContext(DbContextOptions options, ITenantService tenantService) 
        : base(options)
    {
        this.tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
    }

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in this.ChangeTracker.Entries<Entity>())
        {
            entry.Entity.TenantId = entry.State switch
            {
                EntityState.Added => this.tenantService.Tenant,
                _ => entry.Entity.TenantId
            };
        }

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}
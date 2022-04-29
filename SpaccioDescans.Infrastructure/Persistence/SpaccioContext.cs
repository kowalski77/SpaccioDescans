using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Core.Products;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : IdentityDbContext<IdentityUser>, IUnitOfWork
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public SpaccioContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) 
        : base(options)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        builder.Entity<Product>().HasQueryFilter(x => x.TenantId == this.GetTenantId());

        base.OnModelCreating(builder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = this.GetTenantId();
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

    private int GetTenantId()
    {
        var storeClaim = this.httpContextAccessor.HttpContext!.User.Claims.First(x => x.Type == "store");
        var tenantId = int.Parse(storeClaim.Value, CultureInfo.InvariantCulture);

        return tenantId;
    }
}
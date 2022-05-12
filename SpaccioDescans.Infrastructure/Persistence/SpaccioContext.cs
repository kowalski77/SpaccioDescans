using MediatR;
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.Infrastructure.Persistence.Configurations;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : TransactionContext
{
    public SpaccioContext(DbContextOptions options, IMediator mediator)
        : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

        base.OnModelCreating(builder);
    }
}
using System.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SpaccioDescans.Core.Domain.Orders;
using SpaccioDescans.Core.Domain.Products;
using SpaccioDescans.Core.Domain.Stores;
using SpaccioDescans.Infrastructure.Persistence.Configurations;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public class SpaccioContext : IdentityDbContext<IdentityUser>, IDbContext, IUnitOfWork
{
    private readonly IMediator mediator;
    private IDbContextTransaction? currentTransaction;

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

    public DatabaseFacade DatabaseFacade => base.Database;

    public IDbContextTransaction GetCurrentTransaction()
    {
        if (this.currentTransaction is null)
        {
            throw new InvalidOperationException("Current transaction is null");
        }

        return this.currentTransaction;
    }

    public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (this.currentTransaction is not null)
        {
            throw new InvalidOperationException("There is already a transaction in progress.");
        }

        this.currentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken)
            .ConfigureAwait(false);

        return this.currentTransaction;
    }

    public virtual Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction != this.currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        return this.TryCommitTransactionAsync(transaction, cancellationToken);
    }

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

    private async Task TryCommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            this.RollbackTransaction();
            throw;
        }
        finally
        {
            if (this.currentTransaction is not null)
            {
                this.currentTransaction.Dispose();
                this.currentTransaction = null;
            }
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            this.currentTransaction?.Rollback();
        }
        finally
        {
            if (this.currentTransaction is not null)
            {
                this.currentTransaction.Dispose();
                this.currentTransaction = null;
            }
        }
    }
}
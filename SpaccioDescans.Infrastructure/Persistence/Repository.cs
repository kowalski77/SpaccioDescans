using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Persistence;

public abstract class Repository<T> : IRepository<T>
    where T : class, IAggregateRoot
{
    protected Repository(SpaccioContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected SpaccioContext Context { get; }

    public IUnitOfWork UnitOfWork => this.Context;

    public virtual T Save(T item)
    {
        return this.Context.Set<T>().Add(item).Entity;
    }

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>().FindAsync(new object?[] { id }, cancellationToken).ConfigureAwait(false);
    }
}
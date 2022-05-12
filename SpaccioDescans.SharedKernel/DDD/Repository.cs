namespace SpaccioDescans.SharedKernel.DDD;

public abstract class Repository<T> : IRepository<T>
    where T : class, IAggregateRoot
{
    protected Repository(TransactionContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected TransactionContext Context { get; }

    public IUnitOfWork UnitOfWork => this.Context;

    public virtual T Save(T item)
    {
        return this.Context.Set<T>().Add(item).Entity;
    }

    public virtual async Task<T> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>().FindAsync(new object?[] { id }, cancellationToken).ConfigureAwait(false) ??
               throw new InvalidOperationException($"Entity type: {typeof(T).Name} with id: {id} not found");
    }
}
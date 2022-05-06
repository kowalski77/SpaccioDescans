namespace SpaccioDescans.SharedKernel.DDD;

public interface IRepository<T>
    where T : class, IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }

    T Add(T item);

    Task<T> GetAsync(long id, CancellationToken cancellationToken = default);
}
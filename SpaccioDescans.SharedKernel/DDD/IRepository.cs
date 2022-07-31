namespace SpaccioDescans.SharedKernel.DDD;

public interface IRepository<T>
    where T : class, IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }

    T Save(T item);

    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
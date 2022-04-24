using MediatR;

namespace SpaccioDescans.SharedKernel.DDD;

public abstract class Entity
{
    private readonly List<INotification> domainEvents = new();

    public IEnumerable<INotification> DomainEvents => this.domainEvents;

    public bool SoftDeleted { get; protected set; }

    protected void AddDomainEvent(INotification eventItem)
    {
        this.domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        this.domainEvents.Clear();
    }
}

namespace ECommerce.Shared.Core.Events;

public interface IDomainEvent
{    
    DateTime OccurredAt => DateTime.UtcNow;
    Guid EventId => Guid.CreateVersion7();
}

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
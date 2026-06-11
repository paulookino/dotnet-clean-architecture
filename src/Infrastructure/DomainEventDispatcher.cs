using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Infrastructure;

internal sealed class DomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventWrapper<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
            await publisher.Publish(notification, cancellationToken);
        }
    }
}

public sealed class DomainEventWrapper<T>(T domainEvent) : INotification where T : IDomainEvent
{
    public T DomainEvent { get; } = domainEvent;
}

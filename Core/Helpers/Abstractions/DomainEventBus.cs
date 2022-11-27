using System.Collections.Generic;
using System.Linq;

namespace Helpers.Abstractions;

public static class DomainEventBus
{
    private static readonly List<IDomainEvent> _domainEvents = new();

    public static void RaiseEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public static IEnumerable<IDomainEvent> DispatchEvents()
    {
        var domainEvents = _domainEvents.ToList();
        _domainEvents.Clear();
        return domainEvents;
    }
}

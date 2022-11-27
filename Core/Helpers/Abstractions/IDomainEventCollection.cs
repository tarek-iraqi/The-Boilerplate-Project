using System.Collections.Generic;

namespace Helpers.Abstractions;

public interface IDomainEventCollection
{
    List<IDomainEvent> DomainEvents { get; set; }

    void RaiseEvent(IDomainEvent domainEvent);

    IEnumerable<IDomainEvent> DispatchEvents();
}
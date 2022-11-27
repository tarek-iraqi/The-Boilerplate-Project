using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities;

public interface IDomainEventCollection
{
    List<IDomainEvent> DomainEvents { get; set; }

    void RaiseEvent(IDomainEvent domainEvent);

    IEnumerable<IDomainEvent> DispatchEvents();
}
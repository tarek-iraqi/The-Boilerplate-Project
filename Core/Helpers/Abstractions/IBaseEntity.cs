using System;
using System.Collections.Generic;

namespace Helpers.Abstractions;

public interface IBaseEntity
{
    string CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
    string LastModifiedBy { get; set; }
    DateTime? LastModifiedOn { get; set; }
    bool IsDeleted { get; set; }

    List<IDomainEvent> DomainEvents { get; set; }
    void RaiseEvent(IDomainEvent domainEvent);
    IEnumerable<IDomainEvent> DispatchEvents();
}

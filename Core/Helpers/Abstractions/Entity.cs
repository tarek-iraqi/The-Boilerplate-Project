using Helpers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Common;

public abstract class Entity<T> : IBaseEntity, IDomainEventCollection
{
    public T Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<T> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        if (Id.Equals(default(T)) || other.Id.Equals(default(T)))
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<T> a,
                                   Entity<T> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<T> a, Entity<T> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }

    private Type GetRealType()
    {
        Type type = GetType();

        //ToDo: Minor violation in seperation of concerns [entity domain model knows about ORM proxies]
        if (type.ToString().Contains("Castle.Proxies."))
            return type.BaseType;

        return type;
    }

    public List<IDomainEvent> DomainEvents { get; set; }
    public void RaiseEvent(IDomainEvent domainEvent)
    {
        DomainEvents ??= new();
        DomainEvents.Add(domainEvent);
    }

    public IEnumerable<IDomainEvent> DispatchEvents()
    {
        var domainEvents = DomainEvents.ToList();
        DomainEvents.Clear();
        return domainEvents;
    }
}

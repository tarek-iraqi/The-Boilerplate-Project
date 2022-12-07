using Helpers.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities;

public class AppRole : IdentityRole<Guid>, IBaseEntity
{
    public AppRole()
    {
        UserRoles = new HashSet<AppUserRole>();
        RoleClaims = new HashSet<AppRoleClaim>();
    }

    public string Alias { get; private set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool IsDeleted { get; set; }

    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<AppRoleClaim> RoleClaims { get; private set; }

    public void AddPermissions(IEnumerable<int> permissions, string type)
    {
        foreach (var permission in permissions)
        {
            RoleClaims.Add(new AppRoleClaim
            {
                ClaimType = type,
                ClaimValue = permission.ToString(),
            });
        }
    }

    public static AppRole Create(string name, string alias = null)
    {
        return new AppRole
        {
            Name = name,
            Alias = alias ?? name.ToUpper().Replace(" ", "_")
        };
    }

    public List<IDomainEvent> DomainEvents { get; set; }
    public void RaiseEvent(IDomainEvent domainEvent)
    {
        DomainEvents ??= new();
        DomainEvents.Add(domainEvent);
    }

    public IEnumerable<IDomainEvent> DispatchEvents()
    {
        var domainEvents = DomainEvents?.ToList();
        DomainEvents?.Clear();
        return domainEvents;
    }
}
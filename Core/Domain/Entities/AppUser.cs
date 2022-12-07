using Domain.ValueObjects;
using Helpers.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities;

public sealed class AppUser : IdentityUser<Guid>, IBaseEntity
{
    private AppUser()
    {
        Claims = new HashSet<AppUserClaim>();
        Logins = new HashSet<AppUserLogin>();
        Tokens = new HashSet<AppUserToken>();
        UserRoles = new HashSet<AppUserRole>();
        Devices = new HashSet<Device>();
    }

    public AppUser(Name name, string user_name, string email,
        string phone_number = null, string profile_picture = null, bool is_active = true, bool isEmailConfirmed = false)
    {
        Name = name;
        UserName = user_name;
        Email = email;
        PhoneNumber = phone_number;
        ProfilePicture = profile_picture;
        IsActive = is_active;
        EmailConfirmed = isEmailConfirmed;
    }

    public void EditData(Name name, string email, string phoneNumber, string profilePicture)
    {
        Name = name == null ? Name : name;
        Email = string.IsNullOrWhiteSpace(email) ? Email : email;
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? PhoneNumber : phoneNumber;
        ProfilePicture = string.IsNullOrWhiteSpace(profilePicture) ? ProfilePicture : profilePicture;
    }

    public void ChangeProfilePicture(string picture)
    {
        ProfilePicture = string.IsNullOrEmpty(picture) ? ProfilePicture : picture;
    }

    public Name Name { get; private set; }
    public string ProfilePicture { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<AppUserClaim> Claims { get; set; }
    public ICollection<AppUserLogin> Logins { get; set; }
    public ICollection<AppUserToken> Tokens { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
    public ICollection<Device> Devices { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool IsDeleted { get; set; }

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
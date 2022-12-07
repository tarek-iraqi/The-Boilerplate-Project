using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities;

public class AppUserClaim : IdentityUserClaim<Guid>
{
    public virtual AppUser User { get; set; }
}
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities;

public class AppUserToken : IdentityUserToken<Guid>
{
    public virtual AppUser User { get; set; }
}
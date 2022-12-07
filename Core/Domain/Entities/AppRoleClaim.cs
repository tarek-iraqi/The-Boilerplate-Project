using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities;

public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual AppRole Role { get; set; }
}
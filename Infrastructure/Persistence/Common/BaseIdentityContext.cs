using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence.Common
{
    public abstract class BaseIdentityContext : IdentityDbContext<AppUser, AppRole, Guid,
        AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        protected BaseIdentityContext(DbContextOptions options) : base(options)
        {
        }
    }
}

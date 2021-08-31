using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

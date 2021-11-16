using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class AppUserLogin : IdentityUserLogin<Guid>
    {
        public virtual AppUser User { get; set; }
    }
}

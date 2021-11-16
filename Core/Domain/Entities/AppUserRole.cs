using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}

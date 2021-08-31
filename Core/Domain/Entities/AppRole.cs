using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppRole : IdentityRole<Guid>, IBaseEntity
    {
        public AppRole()
        {
            UserRoles = new HashSet<AppUserRole>();
            RoleClaims = new HashSet<AppRoleClaim>(); 
        }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; }
    }
}

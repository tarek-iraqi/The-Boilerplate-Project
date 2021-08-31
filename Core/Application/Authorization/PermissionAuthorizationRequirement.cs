using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(string permissions)
        {
            Permissions = permissions;
        }

        public string Permissions { get; }
    }
}

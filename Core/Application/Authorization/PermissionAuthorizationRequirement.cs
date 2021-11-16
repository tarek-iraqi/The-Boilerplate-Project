using Microsoft.AspNetCore.Authorization;

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

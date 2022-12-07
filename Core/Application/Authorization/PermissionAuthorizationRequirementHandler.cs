using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Authorization;

public class PermissionAuthorizationRequirementHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        var isAuthorized = context.User.IsInRole(DefaultRoles.SUPER_ADMIN.ToString());

        if (isAuthorized)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        if (requirement.Permissions.Contains(PermissionConstants.PermissionGroupSeparator))
        {
            var permissionGroupsString = requirement.Permissions.Split(PermissionConstants.PermissionSeparator);

            var permissionGroups = permissionGroupsString
                .Select(x => x.Split(PermissionConstants.PermissionGroupSeparator).Select(int.Parse));

            foreach (var group in permissionGroups)
            {
                isAuthorized = !group.Except(
                    context.User.Claims.Where(x => x.Type == PermissionConstants.ActionPermission)
                        .Select(x => int.Parse(x.Value))).Any();

                if (isAuthorized) break;
            }
        }
        else if (requirement.Permissions.Contains(PermissionConstants.PermissionSeparator))
        {
            var permissions = requirement.Permissions
                .Split(PermissionConstants.PermissionSeparator)
                .Select(int.Parse);

            isAuthorized = context.User.Claims
                .Where(x => x.Type == PermissionConstants.ActionPermission)
                .Any(x => permissions.Contains(int.Parse(x.Value)));
        }
        else
        {
            var permission = int.Parse(requirement.Permissions);

            isAuthorized = context.User.Claims
                .Any(x => x.Type == PermissionConstants.ActionPermission && int.Parse(x.Value) == permission);
        }

        if (isAuthorized)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
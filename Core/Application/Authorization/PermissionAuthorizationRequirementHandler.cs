using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public class PermissionAuthorizationRequirementHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            string[] claims = null;
            string compareOperator = null;

            if (requirement.Permissions.Contains(PermissionConstants.PermissionCompareOperatorSeparator))
            {
                var tempArray = requirement.Permissions.Split(PermissionConstants.PermissionCompareOperatorSeparator);
                claims = tempArray[0].Split(PermissionConstants.PermissionSeparator);
                compareOperator = tempArray[1];
            }
            else
            {
                claims = requirement.Permissions.Split(PermissionConstants.PermissionSeparator);
            }

            bool isAuthorized = context.User.IsInRole(DefaultRoles.SUPER_ADMIN.ToString());

            if (!isAuthorized)
            {
                if (!string.IsNullOrEmpty(compareOperator) &&
                    (PermissionCompareOperator)Enum.Parse(typeof(PermissionCompareOperator), compareOperator) == PermissionCompareOperator.Or)
                {
                    foreach (var claim in claims)
                    {
                        int claimValue = (int)((Permissions)Enum.Parse(typeof(Permissions), claim));

                        isAuthorized = context.User.Claims.Any(a =>
                            a.Type == PermissionConstants.ActionPermission && int.Parse(a.Value) == claimValue);

                        if (isAuthorized) break;
                    }
                }
                else
                {
                    foreach (var claim in claims)
                    {
                        Enum.TryParse(typeof(Permissions), claim, out object permisssion);

                        if (permisssion is not null)
                        {
                            int claimValue = (int)((Permissions)permisssion);

                            isAuthorized = context.User.Claims.Any(a =>
                                a.Type == PermissionConstants.ActionPermission && int.Parse(a.Value) == claimValue);
                        }

                        if (!isAuthorized) break;
                    }
                }
            }

            if (isAuthorized)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

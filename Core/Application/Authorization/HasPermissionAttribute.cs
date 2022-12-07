using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Helpers.Extensions;
using Microsoft.VisualBasic;

namespace Application.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permission) : base(permission.ToString())
    {}

    public HasPermissionAttribute(params Permissions[] permissions)
        : base(permissions
            .Select(p => p.ToString())
            .Aggregate((a, b) => a + PermissionConstants.PermissionSeparator + b))
    {}
    
    public HasPermissionAttribute(params PermissionGroup[] permissionGroups)
        : base(permissionGroups
            .Select(g => g.GetAttribute<EnumPermissionGroupAttribute>().ToString())
            .Aggregate((a, b) => a + PermissionConstants.PermissionSeparator + b))
    { }
}
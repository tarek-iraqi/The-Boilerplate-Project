using System;
using System.Linq;
using Microsoft.VisualBasic;

namespace Application.Authorization;

internal class EnumPermissionGroupAttribute : Attribute
{
    internal EnumPermissionGroupAttribute(params Permissions[] permissions)
    {
        Permissions = permissions;
    }

    private Permissions[] Permissions { get; }

    public override string ToString()
    {
        return Permissions
            .Select(p => p.ToStringNumber())
            .Aggregate((a, b) => a + PermissionConstants.PermissionGroupSeparator + b);
    }
}
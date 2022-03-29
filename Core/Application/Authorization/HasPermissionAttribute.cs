using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace Application.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permissions permission) : base(permission.ToString())
        {}

        public HasPermissionAttribute(PermissionCompareOperator compareOperator, params Permissions[] permissions)
            : base(permissions.Select(p => p.ToString()).Aggregate((a, b) => a + PermissionConstants.PermissionSeparator + b)
                  + PermissionConstants.PermissionCompareOperatorSeparator + compareOperator)
        {}
    }
}

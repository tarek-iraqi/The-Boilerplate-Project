using Application.Authorization;
using System;

namespace Application.Features.RoleManagement.Queries
{
    public class RolesWithPermissionsDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Permissions[] permissions { get; set; }
    }
}

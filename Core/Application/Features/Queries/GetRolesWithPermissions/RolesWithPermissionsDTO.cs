using Application.Authorization;
using System;

namespace Application.Features.Queries.GetRolesWithPermissions;

public class RolesWithPermissionsDTO
{
    public Guid id { get; set; }
    public string name { get; set; }
    public Permissions[] permissions { get; set; }
}
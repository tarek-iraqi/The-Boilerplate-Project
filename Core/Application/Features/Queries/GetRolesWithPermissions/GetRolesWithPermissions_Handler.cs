using Application.Authorization;
using Application.Features.Queries.GetRolesWithPermissions;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Extensions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Queries;

internal class GetRolesWithPermissions_Handler : IRequestHandler<GetRolesWithPermissions_Query,
    OperationResult<PaginatedResult<RolesWithPermissionsDTO>>>
{
    private readonly IUnitOfWork _uow;

    public GetRolesWithPermissions_Handler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<OperationResult<PaginatedResult<RolesWithPermissionsDTO>>> Handle(GetRolesWithPermissions_Query request,
        CancellationToken cancellationToken)
    {
        var result = await _uow.Repository<AppRole>()
        .Entity()
        .Select(role => new RolesWithPermissionsDTO
        {
            id = role.Id,
            name = role.Name,
            permissions = role.RoleClaims
                .Where(claim => claim.ClaimType == PermissionConstants.ActionPermission)
                .Select(claim => (Permissions)Enum.Parse(typeof(Permissions), claim.ClaimValue)).ToArray()
        }).ToPaginatedListAsync(request.page_number, request.page_size, cancellationToken);

        return OperationResult.Success(result);
    }
}

using Application.Features.Queries.GetRolesWithPermissions;
using Helpers.Abstractions;
using Helpers.BaseModels;

namespace Application.Features.Queries;

public record GetRolesWithPermissions_Query(int page_size,
    int page_number) : IQuery<OperationResult<PaginatedResult<RolesWithPermissionsDTO>>>;

using Application.Features.Queries.GetRolesWithPermissions;
using Helpers.BaseModels;
using MediatR;

namespace Application.Features.Queries;

public record GetRolesWithPermissions_Query(int page_size,
    int page_number) : IRequest<OperationResult<PaginatedResult<RolesWithPermissionsDTO>>>;

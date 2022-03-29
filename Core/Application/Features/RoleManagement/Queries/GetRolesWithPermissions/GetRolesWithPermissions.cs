using Application.Authorization;
using Domain.Entities;
using Helpers.Extensions;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.RoleManagement.Queries
{
    public class GetRolesWithPermissions
    {
        public class Query : IRequest<OperationResult>
        {
            public int page_size { get; }
            public int page_number { get; }
            public Query(int pageSize, int pageNumber)
            {
                page_size = pageSize;
                page_number = pageNumber;
            }
        }

        private class Handler : IRequestHandler<Query, OperationResult>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public async Task<OperationResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = _uow.Repository<AppRole>()
                .Entity()
                .Select(role => new RolesWithPermissionsDTO
                {
                    id = role.Id,
                    name = role.Name,
                    permissions = role.RoleClaims
                        .Where(claim => claim.ClaimType == PermissionConstants.ActionPermission)
                        .Select(claim => (Permissions)Enum.Parse(typeof(Permissions), claim.ClaimValue)).ToArray()
                }).ToPaginatedList(request.page_number, request.page_size);

                return OperationResult.Success(result);
            }
        }
    }
}

using Application.DTOs;
using Application.Specifications.Users;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.BaseModels;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Queries;

internal class GetUsersList_Handler : IQueryHandler<GetUsersList_Query, OperationResult<PaginatedResult<UsersListResponseDTO>>>
{
    private readonly IUnitOfWork _uow;

    public GetUsersList_Handler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<OperationResult<PaginatedResult<UsersListResponseDTO>>> Handle(GetUsersList_Query request,
        CancellationToken cancellationToken)
    {
        var result = await _uow.Repository<AppUser>()
                    .PaginatedListAsync(new UsersFilteredAndOrderedSpec(request.name, request.sort_by, request.sort_order),
                            request.page_number, request.page_size);

        return OperationResult.Success(result);
    }
}
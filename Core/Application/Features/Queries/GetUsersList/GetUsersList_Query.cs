using Application.DTOs;
using Helpers.Abstractions;
using Helpers.BaseModels;

namespace Application.Features.Queries;

public record GetUsersList_Query(string name,
    string sort_by,
    string sort_order,
    int page_number,
    int page_size) : IQuery<OperationResult<PaginatedResult<UsersListResponseDTO>>>;

using Application.DTOs;
using Helpers.BaseModels;
using MediatR;

namespace Application.Features.Queries;

public record GetUsersList_Query(string name,
    string sort_by,
    string sort_order,
    int page_number,
    int page_size) : IRequest<OperationResult<PaginatedResult<UsersListResponseDTO>>>;

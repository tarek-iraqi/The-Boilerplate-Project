using Application.DTOs;
using Helpers.BaseModels;
using MediatR;

namespace Application.Features.Queries;

public record GetUserDevices_Query(int page_size,
    int page_number) : IRequest<OperationResult<PaginatedResult<UserDeviceResponseDTO>>>;

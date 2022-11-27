using Application.DTOs;
using Helpers.Abstractions;
using Helpers.BaseModels;

namespace Application.Features.Queries;

public record GetUserDevices_Query(int page_size,
    int page_number) : IQuery<OperationResult<PaginatedResult<UserDeviceResponseDTO>>>;

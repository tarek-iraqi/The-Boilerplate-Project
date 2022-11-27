using Application.Contracts;
using Application.DTOs;
using Application.Specifications.Devices;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Queries;

internal class GetUserDevices_Handler : IRequestHandler<GetUserDevices_Query,
    OperationResult<PaginatedResult<UserDeviceResponseDTO>>>
{
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _identityService;
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public GetUserDevices_Handler(IUnitOfWork uow, IIdentityService identityService, IAuthenticatedUserService authenticatedUserService)
    {
        _uow = uow;
        _identityService = identityService;
        _authenticatedUserService = authenticatedUserService;
    }
    public async Task<OperationResult<PaginatedResult<UserDeviceResponseDTO>>> Handle(GetUserDevices_Query request,
        CancellationToken cancellationToken)
    {
        var user = await _identityService.FindById(_authenticatedUserService.UserId);

        if (user == null)
            return OperationResult.Fail<PaginatedResult<UserDeviceResponseDTO>>(ErrorStatusCodes.NotFound,
                OperationError.Add(KeyValueConstants.GeneralError, LocalizationKeys.UserNotFound));

        var result = await _uow.Repository<Device>()
            .PaginatedListAsync(new DevicesFilteredByUserSpec(Guid.Parse(_authenticatedUserService.UserId)),
                request.page_number, request.page_size);

        return OperationResult.Success(result);
    }
}
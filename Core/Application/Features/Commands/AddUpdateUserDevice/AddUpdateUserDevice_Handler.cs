﻿using Application.Contracts;
using Application.Specifications.Devices;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class AddUpdateUserDevice_Handler : ICommandHandler<AddUpdateUserDevice_Command, OperationResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _identityService;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IApplicationLocalization _localizer;

    public AddUpdateUserDevice_Handler(IUnitOfWork uow,
        IIdentityService identityService,
        IAuthenticatedUserService authenticatedUserService,
        IApplicationLocalization localizer)
    {
        _uow = uow;
        _identityService = identityService;
        _authenticatedUserService = authenticatedUserService;
        _localizer = localizer;
    }

    public async Task<OperationResult> Handle(AddUpdateUserDevice_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindById(_authenticatedUserService.UserId);

        if (user == null)
            return OperationResult.Fail(ErrorStatusCodes.NotFound,
                OperationError.Add(KeyValueConstants.GeneralError, LocalizationKeys.UserNotFound));

        var device = await _uow.Repository<Device>()
            .GetBySpecAsync(new UserDeviceFilteredByModelOrTokenSpec(Guid.Parse(_authenticatedUserService.UserId),
                request.model, request.token));

        if (device == null)
        {
            _uow.Repository<Device>().Add(new Device
            {
                UserId = Guid.Parse(_authenticatedUserService.UserId),
                Model = request.model,
                Token = request.token,
                DeviceLanguage = _localizer.CurrentLangWithCountry
            });
        }
        else
        {
            device.Model = request.model;
            device.Token = request.token;
            device.DeviceLanguage = _localizer.CurrentLangWithCountry;
            _uow.Repository<Device>().Update(device);
        }

        await _uow.CompleteAsync();

        return OperationResult.Success();
    }
}

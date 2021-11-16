using Application.Interfaces;
using Application.Specifications.Devices;
using Domain.Entities;
using FluentValidation;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Interfaces;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class AddUpdateUserDevice
    {
        public class Command : IRequest
        {
            public string model { get; set; }
            public string token { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.model)
                    .NotEmpty()
                    .WithName(p => localizer.Get(ResourceKeys.DeviceModel));

                RuleFor(p => p.token)
                    .NotEmpty()
                    .WithName(p => localizer.Get(ResourceKeys.DeviceToken));
            }
        }

        private class Handler : IRequestHandler<Command>
        {
            private readonly IUnitOfWork _uow;
            private readonly IIdentityService _identityService;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow,
                IIdentityService identityService,
                IAuthenticatedUserService authenticatedUserService,
                IApplicationLocalization localizer)
            {
                _uow = uow;
                _identityService = identityService;
                _authenticatedUserService = authenticatedUserService;
                _localizer = localizer;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindById(_authenticatedUserService.UserId);

                if (user == null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> { new Tuple<string, string>(KeyValueConstants.GeneralError,
                                    ResourceKeys.UserNotFound) });

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
                return Unit.Value;
            }
        }
    }
}

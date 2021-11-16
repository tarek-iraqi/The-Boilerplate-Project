using Application.DTOs;
using Application.Interfaces;
using Application.Specifications.Devices;
using Domain.Entities;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Interfaces;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Queries
{
    public class GetUserDevices
    {
        public class Query : IRequest<PaginatedResult<UserDeviceResponseDTO>>
        {
            public int page_size { get; }
            public int page_number { get; set; }
            public Query(int pageSize, int pageNumber)
            {
                page_size = pageSize;
                page_number = pageNumber;
            }
        }

        private class Handler : IRequestHandler<Query, PaginatedResult<UserDeviceResponseDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IIdentityService _identityService;
            private readonly IAuthenticatedUserService _authenticatedUserService;

            public Handler(IUnitOfWork uow, IIdentityService identityService, IAuthenticatedUserService authenticatedUserService)
            {
                _uow = uow;
                _identityService = identityService;
                _authenticatedUserService = authenticatedUserService;
            }
            public async Task<PaginatedResult<UserDeviceResponseDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindById(_authenticatedUserService.UserId);

                if (user == null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> { new Tuple<string, string>(KeyValueConstants.GeneralError,
                                    ResourceKeys.UserNotFound) });

                return _uow.Repository<Device>()
                    .PaginatedList(new DevicesFilteredByUserSpec(Guid.Parse(_authenticatedUserService.UserId)),
                        request.page_number, request.page_size);
            }
        }
    }
}

using Application.Contracts;
using Application.Specifications.Devices;
using Domain.Contracts;
using Domain.Entities;
using Helpers.Constants;
using Helpers.Interfaces;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Queries
{
    public class GetUserDevices
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
            private readonly IIdentityService _identityService;
            private readonly IAuthenticatedUserService _authenticatedUserService;

            public Handler(IUnitOfWork uow, IIdentityService identityService, IAuthenticatedUserService authenticatedUserService)
            {
                _uow = uow;
                _identityService = identityService;
                _authenticatedUserService = authenticatedUserService;
            }
            public async Task<OperationResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindById(_authenticatedUserService.UserId);

                if (user == null)
                    return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                        OperationError.Add(KeyValueConstants.GeneralError, LocalizationKeys.UserNotFound));

                var result = _uow.Repository<Device>()
                    .PaginatedList(new DevicesFilteredByUserSpec(Guid.Parse(_authenticatedUserService.UserId)),
                        request.page_number, request.page_size);

                return OperationResult.Success(result);
            }
        }
    }
}

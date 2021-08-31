using Application.DTOs;
using Application.Interfaces;
using Application.Specifications.Users;
using Domain.Entities;
using Helpers.Constants;
using Helpers.Extensions;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Queries
{
    public class UsersList
    {
        public class Query : IRequest<PaginatedResult<UsersListResponseDTO>>
        {
            public string name { get; }
            public string sort_by { get; }
            public string sort_order { get; }
            public int page_number { get; }
            public int page_size { get; }

            public Query(string name, string sortBy, string sortOrder, int pageNumber, int pageSize)
            {
                this.name = name;
                sort_by = sortBy;
                sort_order = sortOrder;
                page_number = pageNumber;
                page_size = pageSize;
            }
        }

        private class Handler : IRequestHandler<Query, PaginatedResult<UsersListResponseDTO>>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public Task<PaginatedResult<UsersListResponseDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = _uow.Repository<AppUser>()
                    .PaginatedList(new UsersFilteredAndOrderedSpec(request.name, request.sort_by, request.sort_order),
                            request.page_number, request.page_size);

                return Task.FromResult(result);
            }
        }
    }
}

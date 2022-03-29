using Application.Specifications.Users;
using Domain.Entities;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Queries
{
    public class UsersList
    {
        public class Query : IRequest<OperationResult>
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

        private class Handler : IRequestHandler<Query, OperationResult>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public Task<OperationResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = _uow.Repository<AppUser>()
                    .PaginatedList(new UsersFilteredAndOrderedSpec(request.name, request.sort_by, request.sort_order),
                            request.page_number, request.page_size);

                return Task.FromResult(OperationResult.Success(result));
            }
        }
    }
}

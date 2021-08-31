using Application.DTOs;
using Helpers.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Models;
using Domain.Entities;
using Application.Specifications.Products;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetPaginatedList 
    {
        public class Query : IRequest<PaginatedResult<ProductResponseDTO>>
        {
            public int page_size { get; }
            public int page_number { get; }

            public Query(int pageSize, int pageNumber)
            {
                page_size = pageSize;
                page_number = pageNumber;
            }
        }

        private class Handler : IRequestHandler<Query, PaginatedResult<ProductResponseDTO>>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public Task<PaginatedResult<ProductResponseDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                return  Task.FromResult(_uow.Repository<Product>().PaginatedList(new ProductOrderedListSpec(),
                    request.page_number, request.page_size));
            }
        }
    }
}

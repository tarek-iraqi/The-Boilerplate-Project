using Application.DTOs;
using Helpers.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Models;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetAll
    {
        public class Query : IRequest<Result<IEnumerable<ProductResponseDTO>>>
        {

        }

        private class Handler : IRequestHandler<Query, Result<IEnumerable<ProductResponseDTO>>>
        {
            private readonly IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public async Task<Result<IEnumerable<ProductResponseDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result =  (await _uow.Repository<Product>().ListAsync()).Select(a => new ProductResponseDTO
                {
                    id = a.Id,
                    name = a.Name,
                    rate = a.Rate
                });

                return new Result<IEnumerable<ProductResponseDTO>>
                {
                    Data = result
                };
            }
        }
    }
}

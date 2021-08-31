using Helpers.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;

namespace Application.Features.ProductFeatures.Queries
{
    public class GetById
    {
        public class Query : IRequest<Product>
        {
            public int id { get; }
            public Query(int id)
            {
                this.id = id;
            }
        }

        private class Handler : IRequestHandler<Query, Product>
        {
            private readonly IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public async Task<Product> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _uow.Repository<Product>().GetByIdAsync(request.id);
                if (product == null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                       new List<Tuple<string, string>> { new Tuple<string, string>(ResourceKeys.Product, ResourceKeys.DataNotFound)});

                return product;
            }
        }
    }
}

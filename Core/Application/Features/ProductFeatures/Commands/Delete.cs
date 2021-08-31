using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using Domain.Entities;
using Helpers.Interfaces;

namespace Application.Features.ProductFeatures.Commands
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int id { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            private readonly IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _uow.Repository<Product>().GetByIdAsync(request.id);

                if (product == null)
                    throw new AppCustomException(ErrorStatusCodes.NotFound,
                        new List<Tuple<string, string>> { new Tuple<string, string>(ResourceKeys.Product, ResourceKeys.DataNotFound) });

                _uow.Repository<Product>().Remove(product);
                await _uow.CompleteAsync();
                return Unit.Value;
            }
        }
    }
}

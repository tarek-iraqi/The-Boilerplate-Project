using Helpers.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Features.ProductFeatures.Commands
{
    public class Update
    {
        public class Command: IRequest
        {
            [JsonIgnore]
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
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
                       new List<Tuple<string, string>> { new Tuple<string, string>(ResourceKeys.Product, ResourceKeys.DataNotFound)});

                product.Update(request.name, request.description);
                await _uow.CompleteAsync();
                return Unit.Value;             
            }
        }
    }
}

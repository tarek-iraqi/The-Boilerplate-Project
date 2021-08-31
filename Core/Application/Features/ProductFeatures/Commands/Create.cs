using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Resources;
using Helpers.Interfaces;

namespace Application.Features.ProductFeatures.Commands
{
    public class Create
    {
        public class Command : IRequest
        {
            public string name { get; set; }
            public string barcode { get; set; }
            public string description { get; set; }
            public decimal rate { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IStringLocalizer<SharedResource> localizer)
            {
                RuleFor(p => p.name)
                    .NotEmpty()
                    .WithName(x => localizer[ResourceKeys.Name].Value);
            }
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
                var product = new Product();
                product.Create(request.name, request.description, request.barcode, request.rate);
                _uow.Repository<Product>().Add(product);
                await _uow.CompleteAsync();
                return Unit.Value;
            }
        }
    }
}

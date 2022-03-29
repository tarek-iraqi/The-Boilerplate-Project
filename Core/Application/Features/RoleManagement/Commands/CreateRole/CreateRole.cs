using Application.Authorization;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using Helpers.Constants;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.RoleManagement.Commands
{
    public class CreateRole
    {
        public class Command : IRequest<OperationResult>
        {
            public string name { get; set; }
            public Permissions[] permissions { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(prop => prop.name)
                    .NotEmpty()
                    .MaximumLength(85)
                    .WithName(x => localizer.Get(LocalizationKeys.RoleName));

                RuleFor(prop => prop.permissions)
                    .ForEach(p => p.IsInEnum())
                    .When(prop => prop.permissions.Length > 0)
                    .WithName(x => localizer.Get(LocalizationKeys.Permissions));
            }
        }

        private class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var isRoleExist = await _identityService.GetRole(request.name);

                if (isRoleExist != null)
                    return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                        OperationError.Add(nameof(request.name), LocalizationKeys.DuplicateRole));

                var newRole = AppRole.Create(request.name);
                newRole.AddPermissions(request.permissions.Select(p => (int)p), PermissionConstants.ActionPermission);

                var result = await _identityService.AddNewRole(newRole);

                return !result.success
                    ? OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                        result.errors.Select(err => OperationError.Add(err.Item1, err.Item2)).ToArray())
                    : OperationResult.Success();
            }
        }
    }
}

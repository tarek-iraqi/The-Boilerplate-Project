using Application.Authorization;
using Application.Contracts;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class CreateRole_Handler : ICommandHandler<CreateRole_Command, OperationResult>
{
    private readonly IIdentityService _identityService;

    public CreateRole_Handler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<OperationResult> Handle(CreateRole_Command request, CancellationToken cancellationToken)
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

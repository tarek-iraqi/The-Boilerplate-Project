using Application.Contracts;
using Domain.ValueObjects;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class UpdateProfile_Handler : ICommandHandler<UpdateProfile_Command, OperationResult>
{
    private readonly IIdentityService _identityService;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IPhoneValidator _phoneValidator;

    public UpdateProfile_Handler(IIdentityService identityService, IAuthenticatedUserService authenticatedUserService,
        IPhoneValidator phoneValidator)
    {
        _identityService = identityService;
        _authenticatedUserService = authenticatedUserService;
        _phoneValidator = phoneValidator;
    }
    public async Task<OperationResult> Handle(UpdateProfile_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindById(_authenticatedUserService.UserId);

        if (user == null)
            return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                OperationError.Add(KeyValueConstants.GeneralError, LocalizationKeys.UserNotFound));

        var phoneInternationalFormat = string.IsNullOrWhiteSpace(request.mobile_number) ? null
            : _phoneValidator.GetInternationalPhoneNumberFormat(request.mobile_number, request.country_code);

        var name = Name.Create(request.first_name, request.last_name);

        user.EditData(name, request.email, phoneInternationalFormat, null);

        var result = await _identityService.Update(user);

        return result.success
           ? OperationResult.Success()
           : OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
               result.errors.Select(err => OperationError.Add(err.Item1, err.Item2)).ToArray());
    }
}

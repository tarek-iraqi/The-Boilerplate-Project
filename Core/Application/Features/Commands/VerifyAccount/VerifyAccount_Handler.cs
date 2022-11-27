using Application.Contracts;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class VerifyAccount_Handler : ICommandHandler<VerifyAccount_Command, OperationResult>
{
    private readonly IIdentityService _identityService;

    public VerifyAccount_Handler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<OperationResult> Handle(VerifyAccount_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByEmail(request.email);

        if (user == null)
            return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                OperationError.Add(nameof(request.email), LocalizationKeys.UserNotFound));

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));

        var result = await _identityService.VerifyUserAccount(user, code);

        return result.success
            ? OperationResult.Success()
            : OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                result.errors.Select(err => OperationError.Add(err.Item1, err.Item2)).ToArray());
    }
}

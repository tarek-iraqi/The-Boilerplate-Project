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

internal class ResetPassword_Handler : ICommandHandler<ResetPassword_Command, OperationResult>
{
    private readonly IIdentityService _identityService;

    public ResetPassword_Handler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<OperationResult> Handle(ResetPassword_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByEmail(request.email);

        if (user == null)
            return OperationResult.Fail(ErrorStatusCodes.BadRequest,
                OperationError.Add(nameof(request.email), LocalizationKeys.UserNotFound));

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));

        var result = await _identityService.ResetPassword(user, code, request.password);

        return result.success
           ? OperationResult.Success()
           : OperationResult.Fail(ErrorStatusCodes.BadRequest,
               result.errors.Select(err => OperationError.Add(err.Item1, err.Item2)).ToArray());
    }
}

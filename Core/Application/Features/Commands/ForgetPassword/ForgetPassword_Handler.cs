using Application.Contracts;
using Application.DTOs;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class ForgetPassword_Handler : ICommandHandler<ForgetPassword_Command, OperationResult>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailSender _emailSender;
    private readonly IApplicationLocalization _localizer;

    public ForgetPassword_Handler(IIdentityService identityService, IEmailSender emailSender, IApplicationLocalization localizer)
    {
        _identityService = identityService;
        _emailSender = emailSender;
        _localizer = localizer;
    }
    public async Task<OperationResult> Handle(ForgetPassword_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByEmail(request.email);

        if (user != null)
        {
            var token = await _identityService.GenerateForgetPasswordToken(user);

            var emailModel = new ResetPasswordEmailDTO
            {
                name = $"{user.Name.First} {user.Name.Last}",
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))
            };

            await _emailSender.SendSingleEmail(user.Email,
                _localizer.Get(LocalizationKeys.ResetPassword),
                KeyValueConstants.ResetPasswordEmailTemplate,
                emailModel);
        }

        return OperationResult.Success();
    }
}

using Application.DTOs;
using Application.Interfaces;
using FluentValidation;
using Helpers.Constants;
using Helpers.Resources;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class ForgetPassword
    {
        public class Command : IRequest
        {
            public string email { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.email)
                    .NotEmpty().EmailAddress()
                    .WithName(p => localizer.Get(ResourceKeys.Email));
            }
        }

        private class Handler : IRequestHandler<Command>
        {
            private readonly IIdentityService _identityService;
            private readonly IEmailSender _emailSender;
            private readonly IApplicationLocalization _localizer;

            public Handler(IIdentityService identityService, IEmailSender emailSender, IApplicationLocalization localizer)
            {
                _identityService = identityService;
                _emailSender = emailSender;
                _localizer = localizer;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByEmail(request.email);

                if (user == null)
                    return Unit.Value;

                var token = await _identityService.GenerateForgetPasswordToken(user);

                var emailModel = new ResetPasswordEmailDTO
                {
                    name = $"{user.Name.First} {user.Name.Last}",
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))
                };

                await _emailSender.SendSingleEmail(user.Email,
                    _localizer.Get(ResourceKeys.ResetPassword),
                    KeyValueConstants.ResetPasswordEmailTemplate,
                    emailModel);

                return Unit.Value;
            }
        }
    }
}

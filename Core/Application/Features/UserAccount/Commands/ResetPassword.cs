using Application.Interfaces;
using FluentValidation;
using Helpers.Constants;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class ResetPassword
    {
        public class Command : IRequest<OperationResult>
        {
            public string email { get; set; }
            public string token { get; set; }
            public string password { get; set; }
            public string password_confirmation { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.email)
                    .NotEmpty().EmailAddress()
                    .WithName(p => localizer.Get(LocalizationKeys.Email));

                RuleFor(p => p.email)
                    .NotEmpty()
                    .WithName(p => localizer.Get(LocalizationKeys.Token));

                RuleFor(p => p.password).NotEmpty()
                    .MinimumLength(8).WithName(localizer.Get(LocalizationKeys.Password));

                RuleFor(p => p.password_confirmation).NotEmpty()
                    .WithName(localizer.Get(LocalizationKeys.ConfirmPassword));

                RuleFor(x => x).Custom((x, context) =>
                {
                    if (x.password != x.password_confirmation)
                    {
                        context.AddFailure(nameof(x.password_confirmation),
                            localizer.Get(LocalizationKeys.PasswordsNotMatch));
                    }
                });
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
                var user = await _identityService.FindByEmail(request.email);

                if (user == null)
                    return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                        OperationError.Add(nameof(request.email), LocalizationKeys.UserNotFound));

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));

                var result = await _identityService.ResetPassword(user, code, request.password);

                return result.success
                   ? OperationResult.Success()
                   : OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                       result.errors.Select(err => OperationError.Add(err.Item1, err.Item2)).ToArray());
            }
        }
    }
}

using Application.Interfaces;
using FluentValidation;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class ResetPassword
    {
        public class Command : IRequest
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
                    .WithName(p => localizer.Get(ResourceKeys.Email));

                RuleFor(p => p.email)
                    .NotEmpty()
                    .WithName(p => localizer.Get(ResourceKeys.Token));

                RuleFor(p => p.password).NotEmpty()
                    .MinimumLength(8).WithName(localizer.Get(ResourceKeys.Password));

                RuleFor(p => p.password_confirmation).NotEmpty()
                    .WithName(localizer.Get(ResourceKeys.ConfirmPassword));

                RuleFor(x => x).Custom((x, context) =>
                {
                    if (x.password != x.password_confirmation)
                    {
                        context.AddFailure(nameof(x.password_confirmation),
                            localizer.Get(ResourceKeys.PasswordsNotMatch));
                    }
                });
            }
        }

        private class Handler : IRequestHandler<Command>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByEmail(request.email);

                if (user == null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.email),
                                    ResourceKeys.UserNotFound) });

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));

                var result = await _identityService.ResetPassword(user, code, request.password);

                if (!result.success)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute, result.errors);

                return Unit.Value;
            }
        }
    }
}

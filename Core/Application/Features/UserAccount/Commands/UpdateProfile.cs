using Application.Contracts;
using Domain.ValueObjects;
using FluentValidation;
using Helpers.Constants;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class UpdateProfile
    {
        public class Command : IRequest<OperationResult>
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string mobile_number { get; set; }
            public string country_code { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer,
                IPhoneValidator phoneValidator)
            {
                RuleFor(p => p.first_name).NotEmpty()
                    .MaximumLength(100)
                    .WithName(x => localizer.Get(LocalizationKeys.FirstName));

                RuleFor(p => p.last_name).NotEmpty()
                    .MaximumLength(100).WithName(localizer.Get(LocalizationKeys.LastName));

                RuleFor(p => p.email).NotEmpty()
                    .EmailAddress().WithName(localizer.Get(LocalizationKeys.Email));

                RuleFor(x => x).Custom((x, context) =>
                {
                    if (!string.IsNullOrWhiteSpace(x.mobile_number) &&
                        !string.IsNullOrWhiteSpace(x.country_code) &&
                        !phoneValidator.IsValidPhoneNumber(x.mobile_number, x.country_code))
                    {
                        context.AddFailure(nameof(x.mobile_number),
                            localizer.Get(LocalizationKeys.InvalidMobileNumber));
                    }
                });

                RuleFor(p => p.country_code)
                    .NotEmpty()
                    .When(p => !string.IsNullOrWhiteSpace(p.mobile_number))
                    .WithName(localizer.Get(LocalizationKeys.CountryCode));
            }
        }

        private class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IIdentityService _identityService;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IPhoneValidator _phoneValidator;

            public Handler(IIdentityService identityService, IAuthenticatedUserService authenticatedUserService,
                IPhoneValidator phoneValidator)
            {
                _identityService = identityService;
                _authenticatedUserService = authenticatedUserService;
                _phoneValidator = phoneValidator;
            }
            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
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
    }
}

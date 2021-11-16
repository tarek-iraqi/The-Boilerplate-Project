using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
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
    public class Register
    {
        public class Command : IRequest
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string mobile_number { get; set; }
            public string country_code { get; set; }
            public string password { get; set; }
            public string password_confirmation { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer,
                IPhoneValidator phoneValidator)
            {
                RuleFor(p => p.first_name).NotEmpty()
                    .MaximumLength(100)
                    .WithName(x => localizer.Get(ResourceKeys.FirstName));

                RuleFor(p => p.last_name).NotEmpty()
                    .MaximumLength(100).WithName(localizer.Get(ResourceKeys.LastName));

                RuleFor(p => p.email).NotEmpty()
                    .EmailAddress().WithName(localizer.Get(ResourceKeys.Email));

                RuleFor(x => x).Custom((x, context) =>
                {
                    if (!string.IsNullOrWhiteSpace(x.mobile_number) &&
                        !string.IsNullOrWhiteSpace(x.country_code) &&
                        !phoneValidator.IsValidPhoneNumber(x.mobile_number, x.country_code))
                    {
                        context.AddFailure(nameof(x.mobile_number),
                            localizer.Get(ResourceKeys.InvalidMobileNumber));
                    }
                });

                RuleFor(p => p.country_code)
                    .NotEmpty()
                    .When(p => !string.IsNullOrWhiteSpace(p.mobile_number))
                    .WithName(localizer.Get(ResourceKeys.CountryCode));

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

        public class Handler : IRequestHandler<Command>
        {
            private readonly IIdentityService _identityService;
            private readonly IEmailSender _emailSender;
            private readonly IApplicationLocalization _localizer;
            private readonly IPhoneValidator _phoneValidator;

            public Handler(IIdentityService identityService, IEmailSender emailSender,
                IApplicationLocalization localizer, IPhoneValidator phoneValidator)
            {
                _identityService = identityService;
                _emailSender = emailSender;
                _localizer = localizer;
                _phoneValidator = phoneValidator;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByName(request.email);

                if (user != null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.email), ResourceKeys.DuplicateEmail) });

                var phoneInternationalFormat = string.IsNullOrWhiteSpace(request.mobile_number) ? null
                    : _phoneValidator.GetInternationalPhoneNumberFormat(request.mobile_number, request.country_code);

                var name = Name.Create(request.first_name, request.last_name);

                var result = await _identityService.Add(new AppUser(
                    name,
                    request.email,
                    request.email,
                    phoneInternationalFormat
                ), request.password);

                if (!result.success)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute, result.errors);

                var emailModel = new AccountVerificationEmailDTO
                {
                    name = $"{request.first_name} {request.last_name}",
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(result.verification_token))
                };

                await _emailSender.SendSingleEmail(request.email,
                    _localizer.Get(ResourceKeys.AccountConfirmation),
                    KeyValueConstants.AccountVerificationEmailTemplate,
                    emailModel);

                return Unit.Value;
            }
        }
    }
}

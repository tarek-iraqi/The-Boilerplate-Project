using Application.Contracts;
using FluentValidation;
using Helpers.BaseModels;
using Helpers.Localization;
using MediatR;

namespace Application.Features.Commands;

public record UpdateProfile_Command(string first_name,
    string last_name,
    string email,
    string mobile_number,
    string country_code) : IRequest<OperationResult>;

public class UpdateProfile_CommandValidator : AbstractValidator<UpdateProfile_Command>
{
    public UpdateProfile_CommandValidator(IApplicationLocalization localizer,
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

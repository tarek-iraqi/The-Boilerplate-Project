using Application.Contracts;
using FluentValidation;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Localization;

namespace Application.Features.Commands;

public record ResetPassword_Command(string email,
    string token,
    string password,
    string password_confirmation) : ICommand<OperationResult>;

public class ResetPassword_CommandValidator : AbstractValidator<ResetPassword_Command>
{
    public ResetPassword_CommandValidator(IApplicationLocalization localizer)
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

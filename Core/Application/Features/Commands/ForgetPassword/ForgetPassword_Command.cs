using Application.Contracts;
using FluentValidation;
using Helpers.BaseModels;
using Helpers.Localization;
using MediatR;

namespace Application.Features.Commands;

public record ForgetPassword_Command(string email) : IRequest<OperationResult>;

public class ForgetPassword_CommandValidator : AbstractValidator<ForgetPassword_Command>
{
    public ForgetPassword_CommandValidator(IApplicationLocalization localizer)
    {
        RuleFor(p => p.email)
            .NotEmpty().EmailAddress()
            .WithName(p => localizer.Get(LocalizationKeys.Email));
    }
}

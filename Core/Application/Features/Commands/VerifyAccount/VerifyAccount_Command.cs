using Application.Contracts;
using FluentValidation;
using Helpers.BaseModels;
using Helpers.Localization;
using MediatR;

namespace Application.Features.Commands;

public record VerifyAccount_Command(string email,
    string token) : IRequest<OperationResult>;

public class VerifyAccount_CommandValidator : AbstractValidator<VerifyAccount_Command>
{
    public VerifyAccount_CommandValidator(IApplicationLocalization localizer)
    {
        RuleFor(p => p.email)
            .NotEmpty().EmailAddress()
            .WithName(p => localizer.Get(LocalizationKeys.Email));

        RuleFor(p => p.token)
           .NotEmpty()
           .WithName(p => localizer.Get(LocalizationKeys.Token));
    }
}

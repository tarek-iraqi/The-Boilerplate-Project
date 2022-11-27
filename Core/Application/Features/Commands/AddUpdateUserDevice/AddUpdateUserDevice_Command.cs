using Application.Contracts;
using FluentValidation;
using Helpers.BaseModels;
using Helpers.Localization;
using MediatR;

namespace Application.Features.Commands;

public record AddUpdateUserDevice_Command(string model,
    string token) : IRequest<OperationResult>;

public class AddUpdateUserDevice_CommandValidator : AbstractValidator<AddUpdateUserDevice_Command>
{
    public AddUpdateUserDevice_CommandValidator(IApplicationLocalization localizer)
    {
        RuleFor(p => p.model)
            .NotEmpty()
            .WithName(p => localizer.Get(LocalizationKeys.DeviceModel));

        RuleFor(p => p.token)
            .NotEmpty()
            .WithName(p => localizer.Get(LocalizationKeys.DeviceToken));
    }
}

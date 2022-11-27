using Application.Authorization;
using Application.Contracts;
using FluentValidation;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Localization;

namespace Application.Features.Commands;

public record CreateRole_Command(string name,
    Permissions[] permissions) : ICommand<OperationResult>;

public class CreateRole_CommandValidator : AbstractValidator<CreateRole_Command>
{
    public CreateRole_CommandValidator(IApplicationLocalization localizer)
    {
        RuleFor(prop => prop.name)
            .NotEmpty()
            .MaximumLength(85)
            .WithName(x => localizer.Get(LocalizationKeys.RoleName));

        RuleFor(prop => prop.permissions)
            .ForEach(p => p.IsInEnum())
            .When(prop => prop.permissions.Length > 0)
            .WithName(x => localizer.Get(LocalizationKeys.Permissions));
    }
}

using Application.Contracts;
using Application.DTOs;
using FluentValidation;
using Helpers.BaseModels;
using Helpers.Localization;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Commands;

public record Login_Command(string username,
    string password,
    [property: JsonIgnore] string ip_address) : IRequest<OperationResult<LoginResponseDTO>>;

public class Login_CommandValidator : AbstractValidator<Login_Command>
{
    public Login_CommandValidator(IApplicationLocalization localizer)
    {
        RuleFor(p => p.username)
            .NotEmpty().EmailAddress()
            .WithName(p => localizer.Get(LocalizationKeys.Username));

        RuleFor(p => p.password)
            .NotEmpty()
            .WithName(p => localizer.Get(LocalizationKeys.Password));
    }
}

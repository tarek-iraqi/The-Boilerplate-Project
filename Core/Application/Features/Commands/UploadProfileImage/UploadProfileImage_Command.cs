using Application.Contracts;
using FluentValidation;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Localization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Commands;

public record UploadProfileImage_Command(IFormFile file) : ICommand<OperationResult<string>>;

public class UploadProfileImage_CommandValidator : AbstractValidator<UploadProfileImage_Command>
{
    public UploadProfileImage_CommandValidator(IApplicationLocalization localizer)
    {
        RuleFor(p => p.file).NotEmpty().WithName(localizer.Get(LocalizationKeys.File));
    }
}

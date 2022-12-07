using Application.Contracts;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.ValueObjects;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class Registeration_Handler : ICommandHandler<Registeration_Command, OperationResult>
{
    private readonly IIdentityService _identityService;
    private readonly IPhoneValidator _phoneValidator;
    private readonly IUnitOfWork _unitOfWork;

    public Registeration_Handler(IIdentityService identityService,
        IPhoneValidator phoneValidator,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _phoneValidator = phoneValidator;
        _unitOfWork = unitOfWork;
    }
    public async Task<OperationResult> Handle(Registeration_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByName(request.email);

        if (user != null)
            return OperationResult.Fail(ErrorStatusCodes.BadRequest,
                OperationError.Add(nameof(request.email), LocalizationKeys.DuplicateEmail));

        var phoneInternationalFormat = string.IsNullOrWhiteSpace(request.mobile_number) ? null
            : _phoneValidator.GetInternationalPhoneNumberFormat(request.mobile_number, request.country_code);

        var name = Name.Create(request.first_name, request.last_name);

        user = new AppUser(
            name,
            request.email,
            request.email,
            phoneInternationalFormat);

        var result = await _identityService.Add(user, request.password);

        if (!result.success)
            return OperationResult.Fail(ErrorStatusCodes.BadRequest,
                result.errors.Select(err => OperationError.Add(err.Item1, err.Item2)).ToArray());

        user.RaiseEvent(new RegisterUserDomainEvent($"{request.first_name} {request.last_name}",
            request.email,
            WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(result.verification_token))));

        await _unitOfWork.CompleteAsync();

        return OperationResult.Success();
    }
}

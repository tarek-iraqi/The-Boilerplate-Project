using Application.Contracts;
using Application.DTOs;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Localization;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class Login_Handler : IRequestHandler<Login_Command, OperationResult<LoginResponseDTO>>
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationConfiguration _configuration;

    public Login_Handler(IIdentityService identityService, IApplicationConfiguration configuration)
    {
        _identityService = identityService;
        _configuration = configuration;
    }
    public async Task<OperationResult<LoginResponseDTO>> Handle(Login_Command request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByName(request.username);

        if (user == null)
            return OperationResult.Fail<LoginResponseDTO>(ErrorStatusCodes.InvalidAttribute,
                    errors: OperationError.Add(nameof(request.username), LocalizationKeys.InvalidCredentials));

        if (!user.EmailConfirmed)
            return OperationResult.Fail<LoginResponseDTO>(ErrorStatusCodes.InvalidAttribute,
                    errors: OperationError.Add(nameof(request.username), LocalizationKeys.EmailNotConfirmed));

        var result = await _identityService.CheckPassword(user, request.password);

        if (!result)
            return OperationResult.Fail<LoginResponseDTO>(ErrorStatusCodes.InvalidAttribute,
                    errors: OperationError.Add(nameof(request.username), LocalizationKeys.InvalidCredentials));

        var accessToken = await _identityService.GenerateAccessToken(user, request.ip_address);
        var refreshToken = _identityService.GenerateRefreshToken();

        var resultData = new LoginResponseDTO
        {
            user = new LoginUserDataResponseDTO
            {
                id = user.Id.ToString(),
                name = $"{user.Name.First} {user.Name.Last}",
                email = user.Email,
                mobile = user.PhoneNumber
            },
            access_token = accessToken,
            refresh_token = refreshToken,
            token_type = KeyValueConstants.TokenType,
            expires_in = _configuration.GetJwtSettings().DurationInMillisecond
        };

        return resultData;
    }
}

using Application.DTOs;
using Application.Contracts;
using FluentValidation;
using Helpers.Constants;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class Login
    {
        public class Command : IRequest<OperationResult>
        {
            public string username { get; set; }
            public string password { get; set; }

            [JsonIgnore]
            public string ip_address { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.username)
                    .NotEmpty().EmailAddress()
                    .WithName(p => localizer.Get(LocalizationKeys.Username));

                RuleFor(p => p.password)
                    .NotEmpty()
                    .WithName(p => localizer.Get(LocalizationKeys.Password));
            }
        }

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IIdentityService _identityService;
            private readonly IApplicationConfiguration _configuration;

            public Handler(IIdentityService identityService, IApplicationConfiguration configuration)
            {
                _identityService = identityService;
                _configuration = configuration;
            }
            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByName(request.username);

                if (user == null)
                    return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                            OperationError.Add(nameof(request.username), LocalizationKeys.InvalidCredentials));

                if (!user.EmailConfirmed)
                    return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                            OperationError.Add(nameof(request.username), LocalizationKeys.EmailNotConfirmed));

                var result = await _identityService.CheckPassword(user, request.password);

                if (!result)
                    return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                            OperationError.Add(nameof(request.username), LocalizationKeys.InvalidCredentials));

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

                return OperationResult.Success(resultData);
            }
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using FluentValidation;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class Login
    {
        public class Command : IRequest<Result<LoginResponseDTO>>
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
                    .WithName(p => localizer.Get(ResourceKeys.Username));

                RuleFor(p => p.password)
                    .NotEmpty()
                    .WithName(p => localizer.Get(ResourceKeys.Password));
            }
        }

        public class Handler : IRequestHandler<Command, Result<LoginResponseDTO>>
        {
            private readonly IIdentityService _identityService;
            private readonly IApplicationConfiguration _configuration;

            public Handler(IIdentityService identityService, IApplicationConfiguration configuration)
            {
                _identityService = identityService;
                _configuration = configuration;
            }
            public async Task<Result<LoginResponseDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByName(request.username);

                if (user == null)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> {
                        new Tuple<string, string>(nameof(request.username), ResourceKeys.InvalidCredentials)});

                if (!user.EmailConfirmed)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> {
                        new Tuple<string, string>(nameof(request.username), ResourceKeys.EmailNotConfirmed)});

                var result = await _identityService.CheckPassword(user, request.password);

                if (!result)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> {
                        new Tuple<string, string>(nameof(request.username), ResourceKeys.InvalidCredentials)});

                var accessToken = await _identityService.GenerateAccessToken(user, request.ip_address);
                var refreshToken = _identityService.GenerateRefreshToken();

                return Result<LoginResponseDTO>.ValueOf(new LoginResponseDTO
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
                });
            }
        }
    }
}

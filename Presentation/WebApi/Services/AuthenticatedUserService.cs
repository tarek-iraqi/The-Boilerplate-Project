using Application.Interfaces;
using Helpers.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) == null ? null : httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            Username = httpContextAccessor.HttpContext?.User?.FindFirst(KeyValueConstants.UsernameClaimType) == null ? null : httpContextAccessor.HttpContext?.User?.FindFirst(KeyValueConstants.UsernameClaimType).Value;
        }

        public string UserId { get; }
        public string Username { get; }
    }
}

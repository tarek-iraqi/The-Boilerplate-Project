using Application.Contracts;
using Helpers.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebApi.Services;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Username = httpContextAccessor.HttpContext?.User?.FindFirst(KeyValueConstants.UsernameClaimType)?.Value;
    }

    public string UserId { get; }
    public string Username { get; }
}
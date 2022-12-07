using Application.DTOs;
using Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IIdentityService
{
    IQueryable<AppUser> Entities { get; }
    Task<CreateUserResponseDTO> Add(AppUser user, string password);
    Task<IdentityResponseDTO> Update(AppUser user);
    Task<AppUser> FindByName(string userName);
    Task<AppUser> FindById(string id);
    Task<AppUser> FindByEmail(string email);
    Task<bool> CheckPassword(AppUser user, string password);
    Task<IdentitySignInResponseDTO> Login(AppUser user, string password);
    Task<IdentityResponseDTO> VerifyUserAccount(AppUser user, string token);
    Task<string> GenerateForgetPasswordToken(AppUser user);
    Task<IdentityResponseDTO> ResetPassword(AppUser user, string token, string password);
    Task<string> GenerateAccessToken(AppUser user, string ipAddress);
    string GenerateRefreshToken();
    Task<bool> IsUserInRole(Guid userId, string roleName);
    Task AddUserToRole(AppUser user, string roleName);
    Task<AppRole> GetRole(Guid roleId);
    Task<AppRole> GetRole(string roleNameOrAlias);
    Task<IdentityResponseDTO> AddNewRole(AppRole role);

}
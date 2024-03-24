using Map.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Map.Platform.Interfaces;

public interface IAuthPlatform
{
    public Task<SecurityToken> CreateTokenAsync(MapUser user);
    public Task<IdentityResult?> ResetPasswordAsync(MapUser user, string password, string token);
    Task<string> GenerateEmailConfirmationLinkAsync(MapUser user);
    Task<string> GeneratePasswordResetLinkAsync(MapUser user);
    Task<IdentityResult> ConfirmEmailAsync(MapUser user, string token);
    Task<string> GenerateEmailUpdateTokenAsync(MapUser user, string newEmail);
    Task<IdentityResult> UpdateEmailAsync(MapUser user, string newEmail, string token);
}

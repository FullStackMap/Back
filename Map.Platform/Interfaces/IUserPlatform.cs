using Map.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Map.Platform.Interfaces;
public interface IUserPlatform
{
    Task<string> GenerateEmailUpdateTokenAsync(MapUser user, string newEmail);
    Task<IdentityResult> UpdateEmailAsync(MapUser user, string newEmail, string token);
}

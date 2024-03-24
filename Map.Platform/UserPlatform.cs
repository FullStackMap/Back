using Map.Domain.Entities;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Map.Platform;
internal class UserPlatform : IUserPlatform
{
    #region Props

    private readonly UserManager<MapUser> _userManager;


    #endregion

    public UserPlatform(UserManager<MapUser> userManager) => _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


    public async Task<string> GenerateEmailUpdateTokenAsync(MapUser user, string newEmail)
    {
        string changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        return $"{user.Id}?Token={changeEmailToken}&Email={newEmail}";
    }
    public async Task<IdentityResult> UpdateEmailAsync(MapUser user, string newEmail, string token) => await _userManager.ChangeEmailAsync(user, newEmail, token);
}

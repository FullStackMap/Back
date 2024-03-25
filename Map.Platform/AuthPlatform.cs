using Map.Domain.Entities;
using Map.Domain.Settings;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Map.Platform;

internal class AuthPlatform : IAuthPlatform
{
    #region Properties

    private readonly UserManager<MapUser> _userManager;
    private readonly JWTSettings _jwtSettings;
    private readonly RegisterSettings _registerSettings;

    #endregion Properties

    #region Constructor

    public AuthPlatform(UserManager<MapUser> userManager, JWTSettings jwtSettings, RegisterSettings registerSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _registerSettings = registerSettings;
    }

    #endregion Constructor

    #region Public Methods

    public async Task<SecurityToken> CreateTokenAsync(MapUser user)
    {
        IList<string>? userRoles = await _userManager.GetRolesAsync(user);

        List<Claim> authClaims = new()
        {
                new Claim("User", user.UserName),
                new Claim("Email", user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        foreach (string userRole in userRoles)
        {
            authClaims.Add(new Claim("Roles", userRole));
        }

        SymmetricSecurityKey authSigningKey = new(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.DurationTime),
            Issuer = _jwtSettings.ValidIssuer,
            Audience = _jwtSettings.ValidAudience,
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateToken(tokenDescriptor);

    }

    public async Task<string> GenerateEmailConfirmationLinkAsync(MapUser user)
    {
        string registrationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return $"{_registerSettings.ConfirmationEmailUrl}?Token={registrationToken}&Email={user.Email}";
    }

    public async Task<IdentityResult> ConfirmEmailAsync(MapUser user, string token) => await _userManager.ConfirmEmailAsync(user, token);

    public async Task<string> GeneratePasswordResetLinkAsync(MapUser user)
    {
        string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        return $"{_registerSettings.ResetPasswordUrl}?Token={resetPasswordToken}&Email={user.Email}";
    }

    public async Task<IdentityResult?> ResetPasswordAsync(MapUser user, string password, string token) => await _userManager.ResetPasswordAsync(user, token, password);

    #endregion Public Methods
}

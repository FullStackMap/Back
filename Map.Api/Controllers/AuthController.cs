using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.MailTemplate;
using Map.Domain.Entities;
using Map.Domain.Models.Auth;
using Map.Domain.Models.Email;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    #region Props
    private readonly UserManager<MapUser> _userManager;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<ConfirmMailDto> _confirmMailValidator;
    private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
    private readonly IAuthPlatform _authPlatform;
    private readonly IMailPlatform _mailPlatform;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor
    public AuthController(UserManager<MapUser> userManager,
                          IValidator<LoginDto> loginValidator,
                          IValidator<RegisterDto> registerValidator,
                          IMapper mapper,
                          IAuthPlatform authPlatform,
                          IValidator<ConfirmMailDto> confirmMailValidator,
                          IMailPlatform mailPlatform,
                          IValidator<ForgotPasswordDto> forgotPasswordValidator,
                          IValidator<ResetPasswordDto> resetPasswordValidator)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _loginValidator = loginValidator ?? throw new ArgumentNullException(nameof(loginValidator));
        _registerValidator = registerValidator ?? throw new ArgumentNullException(nameof(registerValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(registerValidator));
        _authPlatform = authPlatform ?? throw new ArgumentNullException(nameof(authPlatform));
        _confirmMailValidator = confirmMailValidator ?? throw new ArgumentNullException(nameof(confirmMailValidator));
        _mailPlatform = mailPlatform ?? throw new ArgumentNullException(nameof(mailPlatform));
        _forgotPasswordValidator = forgotPasswordValidator ?? throw new ArgumentNullException(nameof(forgotPasswordValidator));
        _resetPasswordValidator = resetPasswordValidator ?? throw new ArgumentNullException(nameof(resetPasswordValidator));
    }

    #endregion

    /// <summary>
    /// Login user
    /// </summary>
    /// <remarks> {  "email": "antoine.capitain+MapPfe@gmail.com",  "password": "NMdRx$HqyT8jX6" }</remarks>
    /// <param name="loginDto">LoginDto</param>
    [HttpPost]
    [Route("Login")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto loginDto)
    {
        ValidationResult validationResult = _loginValidator.Validate(loginDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        MapUser? user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is not null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = await _authPlatform.CreateTokenAsync(user);
            return new TokenDto(tokenHandler.WriteToken(token));
        }
        else
            return Unauthorized();
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="registerDto"></param>
    [HttpPost]
    [Route("Register")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        ValidationResult validationResult = await _registerValidator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        MapUser user = _mapper.Map<RegisterDto, MapUser>(registerDto);

        IdentityResult? result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        string confirmationLink = await _authPlatform.GenerateEmailConfirmationLinkAsync(user);

        string emailTemplateText = _mailPlatform.GetTemplate(TemplatesName.AccountCreatedMail);

        emailTemplateText = emailTemplateText.Replace("[Username]", user.UserName);
        emailTemplateText = emailTemplateText.Replace("[ConfirmationLink]", confirmationLink);

        MailDto mailDto = new()
        {
            Name = user.UserName,
            Email = user.Email,
            Subject = "Bienvenue sur XXX",
            Body = emailTemplateText
        };

        await _mailPlatform.SendMailAsync(mailDto);

        return Created();
    }


    /// <summary>
    /// Activate Account
    /// </summary>
    /// <param name="confirmMailDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("ConfirmEmail")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmMailDto confirmMailDto)
    {
        ValidationResult validationResult = await _confirmMailValidator.ValidateAsync(confirmMailDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        MapUser? user = await _userManager.FindByEmailAsync(confirmMailDto.Email);
        if (user is null)
            BadRequest();

        IdentityResult result = await _authPlatform.ConfirmEmailAsync(user, confirmMailDto.Token);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, Roles.User);

        return result.Succeeded ? Ok() : Unauthorized();
    }


    /// <summary>
    /// Send Forgot Password Mail to user
    /// </summary>
    /// <param name="forgotPasswordDto">ForgotPasswordDto</param>
    [HttpPost]
    [Route("ForgotPassword")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        ValidationResult validationResult = _forgotPasswordValidator.Validate(forgotPasswordDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        MapUser? user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user is null)
            BadRequest();

        string resetPasswordLink = await _authPlatform.GeneratePasswordResetLinkAsync(user);

        string emailTemplateText = _mailPlatform.GetTemplate(TemplatesName.ForgotPasswordMail)!;

        emailTemplateText = emailTemplateText.Replace("[Username]", user.UserName);
        emailTemplateText = emailTemplateText.Replace("[ResetURL]", resetPasswordLink);

        MailDto mailDTO = new()
        {
            Name = user.UserName,
            Email = user.Email,
            Subject = "Changement de mot de passe",
            Body = emailTemplateText
        };

        await _mailPlatform.SendMailAsync(mailDTO);

        return Ok();
    }

    /// <summary>
    /// Reset password of user with token
    /// </summary>
    /// <param name="resetPasswordDto">ResetPasswordDto</param>
    [HttpPost]
    [Route("ResetPassword")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        ValidationResult validationResult = _resetPasswordValidator.Validate(resetPasswordDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        MapUser? user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user is null)
            return BadRequest();

        IdentityResult? result = await _authPlatform.ResetPasswordAsync(user, resetPasswordDto.Password, resetPasswordDto.Token);
        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
        }

        return result.Succeeded ? Ok() : BadRequest(ModelState);
    }
}

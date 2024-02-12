using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.Models.TripDto;
using Map.API.Validator.TripValidator;
using Map.Domain.Entities;
using Map.Domain.Models.AuthDto;
using Map.Domain.Models.TripDto;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    #region Props
    private readonly UserManager<MapUser> _userManager;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<ConfirmMailDto> _confirmMailValidator;
    private readonly IAuthPlatform _authPlatform;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor
    public AuthController(UserManager<MapUser> userManager, IValidator<RegisterDto> registerValidator, IMapper mapper, IAuthPlatform authPlatform, IValidator<ConfirmMailDto> confirmMailValidator)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _registerValidator = registerValidator ?? throw new ArgumentNullException(nameof(registerValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(registerValidator));
        _authPlatform = authPlatform ?? throw new ArgumentNullException(nameof(authPlatform));
        _confirmMailValidator = confirmMailValidator ?? throw new ArgumentNullException(nameof(confirmMailValidator));
    }

    #endregion

    /// <summary>
    /// Register new ser
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Register")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status201Created)]
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

        //TODO : Add envois de mail
        //string emailTemplateText = _emailProvider.GetTemplate(TemplatePath.Register.ToString());
        //if (emailTemplateText is null)
        //    throw new FileNotFoundException(TemplateMessage.TemplateRegisterNotFound.ToString());

        //emailTemplateText = emailTemplateText.Replace("[username]", user.UserName);
        //emailTemplateText = emailTemplateText.Replace("[email]", user.Email);
        //emailTemplateText = emailTemplateText.Replace("[ConfirmationLink]", confirmationLink);

        //MailDTO mailDTO = new()
        //{
        //    Name = user.UserName,
        //    Email = user.Email,
        //    Subject = "Bienvenue sur GameTrip",
        //    Body = emailTemplateText
        //};

        //await _mailPlatform.SendMailAsync(mailDTO);

        return Created();
    }

    [HttpPost]
    [Route("ConfirmEmail")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    //[ProducesResponseType(typeof(TripDto), StatusCodes.Status201Created)]
    //[ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmMailDto confirmMailDto)
    {
        ValidationResult validationResult = await _confirmMailValidator.ValidateAsync(confirmMailDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        MapUser? user = await _userManager.FindByEmailAsync(confirmMailDto.Email);
        if (user is null)
            Unauthorized();

        IdentityResult result = await _authPlatform.ConfirmEmailAsync(user, confirmMailDto.Token);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, Roles.User);

        return result.Succeeded ? Ok() : Unauthorized();
    }


}

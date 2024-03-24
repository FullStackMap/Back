using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.Extension;
using Map.API.MailTemplate;
using Map.API.Models.TripDto;
using Map.API.Models.UserDto;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AuthDto;
using Map.Domain.Models.EmailDto;
using Map.Domain.Models.Auth;
using Map.Domain.Models.Trip;
using Map.Domain.Models.User;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    #region Props

    private readonly UserManager<MapUser> _userManager;
    private readonly IUserPlatform _userPlatform;
    private readonly IValidator<UpdateUserMailDto> _updateUserMailValidator;
    private readonly IValidator<MailDto> _contactMailValidator;
    private readonly IMailPlatform _mailPlatform;


    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public UserController(ITripPlatform tripPlatform,
                          UserManager<MapUser> userManager,
                          IMapper mapper,
                          IValidator<UpdateUserMailDto> updateUserMailValidator,
                          IUserPlatform userPlatform,
                          IValidator<MailDto> contactMailValidator,
                          IMailPlatform mailPlatform)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _updateUserMailValidator = updateUserMailValidator ?? throw new ArgumentNullException(nameof(updateUserMailValidator));
        _userPlatform = userPlatform ?? throw new ArgumentNullException(nameof(userPlatform));
        _contactMailValidator = contactMailValidator ?? throw new ArgumentNullException(nameof(contactMailValidator));
        _mailPlatform = mailPlatform ?? throw new ArgumentNullException(nameof(mailPlatform));
    }

    #endregion


    /// <summary>
    /// Change user email
    /// </summary>
    /// <param name="userId">Id of user</param>
    /// <param name="updateUserMailDto">UpdateUserMailDto</param>
    /// <returns>MapUserDto with new Mail</returns>
    [Authorize]
    [HttpPatch]
    [Route("{userId}/email")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MapUserDto>> UpdateUserMailAsync([FromRoute] Guid userId, [FromBody] UpdateUserMailDto updateUserMailDto)
    {
        ValidationResult validationResult = await _updateUserMailValidator.ValidateAsync(updateUserMailDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        MapUser? user = await _userManager.FindByEmailAsync(updateUserMailDto.Mail);
        if (user is null || user.Id != userId)
        {
            return BadRequest(new Error(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue(), "Utilisateur non trouvé"));
        }

        string changeEmailToken = await _userPlatform.GenerateEmailUpdateTokenAsync(user, updateUserMailDto.Mail);
        IdentityResult? result = await _userPlatform.UpdateEmailAsync(user, updateUserMailDto.Mail, changeEmailToken);

        return Ok(_mapper.Map<MapUser, MapUserDto>(user));
    }

    [HttpPatch]
    [Route("Contact")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ContactMailAsync([FromBody] MailDto mailDto)
    {
        ValidationResult validationResult = _contactMailValidator.Validate(mailDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        string contactMailTemplate = _mailPlatform.GetTemplate(TemplatesName.ContactMail);

        mailDto.Body = contactMailTemplate.Replace("[Name]", mailDto.Name)
                                          .Replace("[Email]", mailDto.Email)
                                          .Replace("[Subject]", mailDto.Subject)
                                          .Replace("[Body]", mailDto.Body);

        mailDto.Subject = $"Nouvelle demande de contact de la part de : {mailDto.Name}";

        await _mailPlatform.SendMailAsync(mailDto);

        return Ok();
    }
}
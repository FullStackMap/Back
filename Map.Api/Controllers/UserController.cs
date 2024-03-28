using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Auth;
using Map.Domain.Models.Trip;
using Map.Domain.Models.User;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

[Authorize]
[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    #region Props

    private readonly UserManager<MapUser> _userManager;
    private readonly IUserPlatform _userPlatform;
    private readonly IValidator<UpdateUserMailDto> _updateUserMailValidator;
    private readonly IValidator<UpdateUserNameDto> _updateUserNameValidator;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public UserController(ITripPlatform tripPlatform,
                          UserManager<MapUser> userManager,
                          IMapper mapper,
                          IValidator<UpdateUserMailDto> updateUserMailValidator,
                          IUserPlatform userPlatform,
                          IValidator<UpdateUserNameDto> updateUserNameValidator)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _updateUserMailValidator = updateUserMailValidator ?? throw new ArgumentNullException(nameof(updateUserMailValidator));
        _userPlatform = userPlatform ?? throw new ArgumentNullException(nameof(userPlatform));
        _updateUserNameValidator = updateUserNameValidator ?? throw new ArgumentNullException(nameof(updateUserNameValidator));
    }

    #endregion


    /// <summary>
    /// Change user email
    /// </summary>
    /// <param name="userId">Id of user</param>
    /// <param name="updateUserMailDto">UpdateUserMailDto</param>
    /// <returns>MapUserDto with new Mail</returns>
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

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="userId">Id of user</param>
    [HttpGet]
    [Route("{userId}/profile")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(MapUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MapUserDto>> GetUserById([FromRoute] Guid userId)
    {
        MapUser? user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null)
            return BadRequest(new Error(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue(), "Utilisateur non trouvé"));

        return _mapper.Map<MapUser, MapUserDto>(user);
    }


    /// <summary>
    /// Update user name
    /// </summary>
    /// <param name="updateUserNameDto">UpdateUserNameDto</param>
    /// <param name="userId">Id of user</param>
    [HttpPatch]
    [Route("{userId}/Username")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MapUserDto>> UpdateUserName([FromRoute] Guid userId, [FromBody] UpdateUserNameDto updateUserNameDto)
    {
        ValidationResult validationResult = _updateUserNameValidator.Validate(updateUserNameDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        MapUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.Id != userId)
            return BadRequest(new Error(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue(), "Utilisateur non trouvé"));

        user.UserName = updateUserNameDto.UserName;
        await _userManager.UpdateAsync(user);

        return _mapper.Map<MapUser, MapUserDto>(user);
    }

    [HttpDelete]
    [Route("{userId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MapUserDto>> DeleteUserById([FromRoute] Guid userId)
    {
        MapUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null || user.Id != userId)
            return BadRequest(new Error(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue(), "Utilisateur non trouvé"));

        await _userManager.DeleteAsync(user);

        return NoContent();
    }
}

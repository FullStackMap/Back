using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Trip;
using Map.EFCore.Data;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

//[Authorize(Roles = Roles.User)]
[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class TripController : ControllerBase
{
    #region Props

    private readonly ITripPlatform _tripPlatform;
    private readonly UserManager<MapUser> _userManager;

    private readonly IValidator<AddTripDto> _addTripValidator;
    private readonly IValidator<UpdateTripDto> _updateTripValidator;

    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public TripController(ITripPlatform tripPlatform,
                          UserManager<MapUser> userManager,
                          IValidator<AddTripDto> addTripValidator,
                          IValidator<UpdateTripDto> updateTripValidator,
                          IMapper mapper,
                          IOutputCacheStore cahce)
    {
        _tripPlatform = tripPlatform ?? throw new ArgumentNullException(nameof(tripPlatform));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _addTripValidator = addTripValidator ?? throw new ArgumentNullException(nameof(addTripValidator));
        _updateTripValidator = updateTripValidator ?? throw new ArgumentNullException(nameof(updateTripValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion

    /// <summary>
    /// Permet d'initialiser la base de données
    /// </summary>
    /// <param name="force">Cette option supprime intégralement la donnée et réaplique une initialisation par dessus</param>
    [AllowAnonymous]
    [HttpPost]
    [Route("Initialize")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    public async Task<IActionResult> Initialize([FromServices] DBInitializer dBInitializer, [FromQuery] bool force = false)
    {
        bool result = await dBInitializer.Initialize(force);


        string resultMessage = $"""
            Initialisation DB
            -> Result: {(result ? "Succès" : "La DB existe déja")}

            {(!result && !force ? "-> Utilise le query param 'force' pour forcer l'initialisation. Attention, cela supprimera toutes les données de la DB" : null)}
            """;

        return Ok(resultMessage);
    }

    /// <summary>
    /// Create a new trip
    /// </summary>
    /// <param name="addTripDto">addTripDto</param>
    [HttpPost]
    [Route("")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TripDto>> AddTrip([FromBody] AddTripDto addTripDto)
    {
        ValidationResult validationResult = await _addTripValidator.ValidateAsync(addTripDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        if (addTripDto.BackgroundPicturePath is null)
            addTripDto.BackgroundPicturePath = "https://www.voyageursdumonde.fr/voyage-sur-mesure/magazine-voyage/showphoto/1357/0";

        Trip entity = _mapper.Map<AddTripDto, Trip>(addTripDto);
        await _tripPlatform.AddTripAsync(entity);

        return CreatedAtAction(nameof(GetTripById), new { tripId = entity.TripId }, _mapper.Map<Trip, TripDto>(entity));
    }

    /// <summary>
    /// Get all trip
    /// </summary>
    [HttpGet]
    [Route("")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(IEnumerable<TripDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public IQueryable<TripDto> GetAllTrip()
    {
        IQueryable<Trip> queryTrip = _tripPlatform.GetAll();

        return _mapper.ProjectTo<TripDto>(queryTrip);
    }

    /// <summary>
    /// Get a trip by id
    /// </summary>
    /// <param name="tripId">Id of wanted Trip</param>
    [HttpGet]
    [Route("{tripId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TripDto>> GetTripById([FromRoute] Guid tripId)
    {
        Trip? entity = await _tripPlatform.GetTripByIdAsync(tripId);
        if (entity is null)
            return NotFound(new Error(nameof(ETripErrorCodes.TripNotFoundById), $"Trip with id: {tripId} not found"));

        return _mapper.Map<Trip, TripDto>(entity);
    }

    /// <summary>
    /// Get all trip by user id
    /// </summary>
    /// <param name="userId">User id of wanted Trips</param>
    [HttpGet]
    [Route("User/{userId}")]
    [OutputCache(Tags = [$"Trips/Users/userId"])]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(List<TripDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<TripDto>>> GetAllTripByUserIdAsync([FromRoute] Guid userId)
    {
        List<Trip>? entities = await _tripPlatform.GetTripListByUserIdAsync(userId);
        if (entities is null || !entities.Any())
            return NoContent();

        return _mapper.Map<List<Trip>, List<TripDto>>(entities);
    }

    /// <summary>
    /// Update or Create a trip
    /// </summary>
    /// <param name="tripId">Id of updated trip</param>
    /// <param name="updateTripDto">UpdateTripDto</param>
    /// <returns>Updated trip if tripId founded Or Created Trip</returns>
    [HttpPut]
    [Route("{tripId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TripDto>> UpdateTripAsync([FromRoute] Guid tripId, [FromBody] UpdateTripDto updateTripDto)
    {
        ValidationResult validationResult = await _updateTripValidator.ValidateAsync(updateTripDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        if (updateTripDto.BackgroundPicturePath is null)
            updateTripDto.BackgroundPicturePath = "https://www.voyageursdumonde.fr/voyage-sur-mesure/magazine-voyage/showphoto/1357/0";

        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
        {
            Trip entity = _mapper.Map<UpdateTripDto, Trip>(updateTripDto);
            await _tripPlatform.AddTripAsync(entity);

            return CreatedAtAction(nameof(GetTripById), new { tripId = entity.TripId }, _mapper.Map<Trip, TripDto>(entity));
        }

        Trip? tripUpdate = await _tripPlatform.UpdateTripAsync(trip, updateTripDto);
        return _mapper.Map<Trip, TripDto>(tripUpdate);
    }

    /// <summary>
    /// Delete a trip by id
    /// </summary>
    /// <param name="tripId">Trip Id of delete trip</param>
    [HttpDelete]
    [Route("{tripId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTripByIdAsync([FromRoute] Guid tripId)
    {
        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
            return NotFound(new Error(nameof(ETripErrorCodes.TripNotFoundById), $"Trip with id: {tripId} not found"));

        _tripPlatform.Delete(trip);

        return NoContent();
    }
}

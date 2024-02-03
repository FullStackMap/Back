using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.Models.TripDto;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

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
                          IMapper mapper)
    {
        _tripPlatform = tripPlatform ?? throw new ArgumentNullException(nameof(tripPlatform));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _addTripValidator = addTripValidator ?? throw new ArgumentNullException(nameof(addTripValidator));
        _updateTripValidator = updateTripValidator ?? throw new ArgumentNullException(nameof(updateTripValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion

    //[AllowAnonymous]
    //[HttpPost]
    //[Route("Initialize")]
    //[MapToApiVersion(ApiControllerVersions.V1)]
    //public async Task<IActionResult> Initialize([FromServices] DBInitializer dBInitializer)
    //{
    //    bool result = await dBInitializer.Initialize();
    //    string resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

    //    return Ok(resultMessage);
    //}

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
    public async Task<IActionResult> AddTrip([FromBody] AddTripDto addTripDto)
    {
        ValidationResult validationResult = await _addTripValidator.ValidateAsync(addTripDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

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
    public async Task<IActionResult> GetAllTripAsync()
    {
        List<Trip> entities = await _tripPlatform.GetAllAsync();
        if (entities is null || !entities.Any())
            return NoContent();

        return Ok(_mapper.Map<List<Trip>, List<TripDto>>(entities));
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
    public async Task<IActionResult> GetTripById([FromRoute] Guid tripId)
    {
        Trip? entity = await _tripPlatform.GetTripByIdAsync(tripId);
        if (entity is null)
            return NotFound(new Error(nameof(ETripErrorCodes.TripNotFoundById), $"Trip with id: {tripId} not found"));

        return Ok(_mapper.Map<Trip, TripDto>(entity));
    }

    /// <summary>
    /// Get all trip by user id
    /// </summary>
    /// <param name="userId">User id of wanted Trips</param>
    [HttpGet]
    [Route("User/{userId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTripByUserIdAsync([FromRoute] Guid userId)
    {
        List<Trip>? entities = await _tripPlatform.GetTripListByUserIdAsync(userId);
        if (entities is null || !entities.Any())
            return NoContent();

        return Ok(_mapper.Map<List<Trip>, List<TripDto>>(entities));
    }

    //PUT de Trip
    /// <summary>
    /// definir le verbe http et la route
    /// donné version constrolerapi définir les statusCode 
    /// décalrer methode avec les parametres (si necessaire) créer dto 
    /// définir les règles de validation
    /// appeller l'orm 
    /// retourner le resultat
    /// </summary>
    [HttpPut]
    [Route("{tripId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTripAsync([FromRoute] Guid tripId, [FromBody] UpdateTripDto updateTripDto)
    {
        // validator 
        ValidationResult validationResult = await _updateTripValidator.ValidateAsync(updateTripDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        return Ok();

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

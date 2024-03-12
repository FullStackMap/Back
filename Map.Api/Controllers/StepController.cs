using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Step;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

//[Authorize(Roles = Roles.User)]
[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class StepController : ControllerBase
{
    #region Props
    private readonly IMapper _mapper;
    private readonly IValidator<AddStepDto> _addStepValidator;
    private readonly ITripPlatform _tripPlatform;
    private readonly IStepPlatform _stepPlatform;

    #endregion

    #region Ctor
    public StepController(IMapper mapper, IValidator<AddStepDto> addStepValidator, ITripPlatform tripPlatform, IStepPlatform stepPlatform)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _addStepValidator = addStepValidator ?? throw new ArgumentNullException(nameof(addStepValidator));
        _tripPlatform = tripPlatform ?? throw new ArgumentNullException(nameof(tripPlatform));
        _stepPlatform = stepPlatform ?? throw new ArgumentNullException(nameof(stepPlatform));
    }
    #endregion

    #region Add
    /// <summary>
    /// Add a step to a trip to end of steps in trip
    /// </summary>
    /// <param name="tripId">Trip id to add Step</param>
    /// <param name="addStepDto">addStepDto</param>
    [HttpPost]
    [Route("{tripId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> AddStepAsync([FromRoute] Guid tripId, [FromBody] AddStepDto addStepDto)
    {
        ValidationResult validationResult = _addStepValidator.Validate(addStepDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Voyage non trouvé par id"));

        Step entity = _mapper.Map<AddStepDto, Step>(addStepDto);
        await _stepPlatform.AddStepAsync(trip, entity);

        return CreatedAtAction(nameof(GetStepByIdAsync), new { stepId = entity.StepId }, _mapper.Map<Step, StepDto>(entity));
    }

    /// <summary>
    /// Add a step to a trip to end of steps in trip
    /// </summary>
    /// <param name="tripId">Trip id to add Step</param>
    /// <param name="addStepDto">addStepDto</param>
    [HttpPost]
    [Route("{tripId}/before-{stepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> AddStepBeforAsync([FromRoute] Guid tripId, [FromRoute] Guid stepId, [FromBody] AddStepDto addStepDto)
    {
        ValidationResult validationResult = _addStepValidator.Validate(addStepDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Voyage non trouvé par id"));

        Step? nextStep = trip.Steps.FirstOrDefault(s => s.StepId == stepId);
        if (nextStep is null)
            return NotFound(new Error(ETripErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Step entity = _mapper.Map<AddStepDto, Step>(addStepDto);
        await _stepPlatform.AddStepBeforAsync(trip, nextStep, entity);

        return CreatedAtAction(nameof(GetStepByIdAsync), new { stepId = entity.StepId }, _mapper.Map<Step, StepDto>(entity));
    }

    /// <summary>
    /// Add a step to a trip to end of steps in trip
    /// </summary>
    /// <param name="tripId">Trip id to add Step</param>
    /// <param name="addStepDto">addStepDto</param>
    [HttpPost]
    [Route("{tripId}/after-{stepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> AddStepAfterAsync([FromRoute] Guid tripId, [FromRoute] Guid stepId, [FromBody] AddStepDto addStepDto)
    {
        ValidationResult validationResult = _addStepValidator.Validate(addStepDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
        }

        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Voyage non trouvé par id"));

        Step? previousStep = trip.Steps.FirstOrDefault(s => s.StepId == stepId);
        if (previousStep is null)
            return NotFound(new Error(ETripErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Step entity = _mapper.Map<AddStepDto, Step>(addStepDto);
        await _stepPlatform.AddStepAfterAsync(trip, previousStep, entity);

        return CreatedAtAction(nameof(GetStepByIdAsync), new { stepId = entity.StepId }, _mapper.Map<Step, StepDto>(entity));
    }
    #endregion

    #region Get
    /// <summary>
    /// Get step by id
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    [HttpGet]
    [Route("{stepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> GetStepByIdAsync([FromRoute] Guid stepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        return _mapper.Map<Step, StepDto>(step);
    }

    /// <summary>
    /// Get all steps of a trip
    /// </summary>
    /// <param name="tripId">Id of wanted Trip</param>
    /// <returns>Collection of StepDtoList</returns>
    [HttpGet]
    [Route("{tripId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(ICollection<StepDtoList>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ICollection<StepDtoList>>> GetStepsByTripIdAsync([FromRoute] Guid tripId)
    {
        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Voyage non trouvé par id"));

        return Ok(_mapper.Map<ICollection<Step>, ICollection<StepDtoList>>(trip.Steps));
    }
    #endregion

    #region Update - move in the list

    /// <summary>
    /// Move a step to the end of the trip
    /// </summary>
    /// <param name="stepId">Id of the step to move</param>
    /// <returns>Step</returns>
    [HttpPatch]
    [Route("{stepId}/move-end")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> MoveStepToEndAsync([FromRoute] Guid stepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        await _stepPlatform.MoveStepToEndAsync(step);

        return _mapper.Map<Step, StepDto>(step);
    }

    /// <summary>
    /// Move a step before another step
    /// </summary>
    /// <param name="stepId">Id of the step to move</param>
    /// <param name="previousStepId">Id of the step before where to move</param>
    /// <returns>Step</returns>
    [HttpPatch]
    [Route("{stepId}/move-before-{previousStepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> MoveStepBefore([FromRoute] Guid stepId, [FromRoute] Guid previousStepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Step? previousStep = await _stepPlatform.GetStepByIdAsync(previousStepId);
        if (previousStep is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        if (previousStep.TripId != step.TripId)
            return BadRequest(new Error(EStepErrorCodes.StepsNotInSameTrip.ToStringValue(), "Les étapes fournies ne sont pas dans le même voyage"));

        if (previousStep.StepNumber < step.StepNumber)
            return BadRequest(new Error(EStepErrorCodes.StepNumberNotInOrder.ToStringValue(), "L'étape suivante doit être après l'étape actuelle"));

        await _stepPlatform.MoveStepBeforeAsync(step, previousStep);

        return _mapper.Map<Step, StepDto>(step);
    }

    [HttpPatch]
    [Route("{stepId}/move-after-{nextStepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> MoveStepAfter([FromRoute] Guid stepId, [FromRoute] Guid nextStepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Step? nextStep = await _stepPlatform.GetStepByIdAsync(nextStepId);
        if (nextStep is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        if (nextStep.TripId != step.TripId)
            return BadRequest(new Error(EStepErrorCodes.StepsNotInSameTrip.ToStringValue(), "Les étapes fournies ne sont pas dans le même voyage"));

        if (nextStep.StepNumber > step.StepNumber)
            return BadRequest(new Error(EStepErrorCodes.StepNumberNotInOrder.ToStringValue(), "L'étape suivante doit être après l'étape actuelle"));

        await _stepPlatform.MoveStepAfterAsync(step, nextStep);

        return _mapper.Map<Step, StepDto>(step);
    }

    #endregion

    #region Delete
    /// <summary>
    /// Delete a step by id
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    /// <returns>NoContent</returns>
    [HttpDelete]
    [Route("{stepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteStepByIdAsync([FromRoute] Guid stepId)
    {
        Step? entity = await _stepPlatform.GetStepByIdAsync(stepId);
        if (entity is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        _stepPlatform.DeleteStep(entity);

        return NoContent();
    }
    #endregion
}

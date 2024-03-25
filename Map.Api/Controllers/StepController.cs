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
    private readonly IValidator<UpdateStepNameDto> _updateStepNameValidator;
    private readonly IValidator<UpdateStepDescriptionDto> _updateStepDescriptionValidator;
    private readonly IValidator<UpdateStepDateDto> _updateStepDateValidator;
    private readonly IValidator<UpdateStepLocationDto> _updateStepLocationValidator;
    private readonly ITripPlatform _tripPlatform;
    private readonly IStepPlatform _stepPlatform;

    #endregion

    #region Ctor
    public StepController(IMapper mapper,
                          IValidator<AddStepDto> addStepValidator,
                          ITripPlatform tripPlatform,
                          IStepPlatform stepPlatform,
                          IValidator<UpdateStepDescriptionDto> updateStepDescriptionValidator,
                          IValidator<UpdateStepDateDto> updateStepDateValidator,
                          IValidator<UpdateStepLocationDto> updateStepLocationValidator,
                          IValidator<UpdateStepNameDto> updateStepNameValidator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _addStepValidator = addStepValidator ?? throw new ArgumentNullException(nameof(addStepValidator));
        _tripPlatform = tripPlatform ?? throw new ArgumentNullException(nameof(tripPlatform));
        _stepPlatform = stepPlatform ?? throw new ArgumentNullException(nameof(stepPlatform));
        _updateStepNameValidator = updateStepNameValidator ?? throw new ArgumentNullException(nameof(updateStepNameValidator));
        _updateStepDescriptionValidator = updateStepDescriptionValidator ?? throw new ArgumentNullException(nameof(updateStepDescriptionValidator));
        _updateStepDateValidator = updateStepDateValidator ?? throw new ArgumentNullException(nameof(updateStepDateValidator));
        _updateStepLocationValidator = updateStepLocationValidator ?? throw new ArgumentNullException(nameof(updateStepLocationValidator));
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
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status201Created)]
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
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Trip? trip = await _tripPlatform.GetTripByIdAsync(tripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Aucun voyage n'as été trouver avec cet Id"));

        Step newStep = _mapper.Map<AddStepDto, Step>(addStepDto);

        await _stepPlatform.AddStepAsync(trip, newStep);

        return CreatedAtAction(nameof(GetStepById), new { stepId = newStep.StepId }, _mapper.Map<Step, StepDto>(newStep));
    }

    /// <summary>
    /// Add a step to a trip to end of steps in trip
    /// </summary>
    /// <param name="tripId">Trip id to add Step</param>
    /// <param name="stepId">Step id to add Step before</param>
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
    public async Task<ActionResult<StepDto>> AddStepBeforAsync([FromRoute] Guid tripId, [FromRoute] int stepId, [FromBody] AddStepDto addStepDto)
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

        return CreatedAtAction(nameof(GetStepById), new { stepId = entity.StepId }, _mapper.Map<Step, StepDto>(entity));
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
    public async Task<ActionResult<StepDto>> AddStepAfterAsync([FromRoute] Guid tripId, [FromRoute] int stepId, [FromBody] AddStepDto addStepDto)
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

        return CreatedAtAction(nameof(GetStepById), new { stepId = entity.StepId }, _mapper.Map<Step, StepDto>(entity));
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
    public async Task<ActionResult<StepDto>> GetStepById([FromRoute] int stepId)
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
    [Route("trips/{tripId}")]
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
    public async Task<ActionResult<StepDto>> MoveStepToEndAsync([FromRoute] int stepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Trip? trip = await _tripPlatform.GetTripByIdAsync(step.TripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Aucun voyage lier a cette étape."));

        await _stepPlatform.MoveStepToEndAsync(trip, step);

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
    public async Task<ActionResult<StepDto>> MoveStepBefore([FromRoute] int stepId, [FromRoute] int previousStepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Step? previousStep = await _stepPlatform.GetStepByIdAsync(previousStepId);
        if (previousStep is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Trip? trip = await _tripPlatform.GetTripByIdAsync(step.TripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Aucun voyage lier a cette étape."));

        if (previousStep.TripId != step.TripId)
            return BadRequest(new Error(EStepErrorCodes.StepsNotInSameTrip.ToStringValue(), "Les étapes fournies ne sont pas dans le même voyage"));

        if (previousStep.Order > step.Order)
            return BadRequest(new Error(EStepErrorCodes.OrderNotInOrder.ToStringValue(), "L'étape précédente doit être avant l'étape actuelle"));

        await _stepPlatform.MoveStepBeforeAsync(trip, step, previousStep);

        return _mapper.Map<Step, StepDto>(step);
    }

    /// <summary>
    /// Move a step after another step
    /// </summary>
    /// <param name="stepId">Id of the step to move</param>
    /// <param name="nextStepId">Id of the step after where to move</param>
    /// <returns>Step</returns>
    [HttpPatch]
    [Route("{stepId}/move-after-{nextStepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> MoveStepAfter([FromRoute] int stepId, [FromRoute] int nextStepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Step? nextStep = await _stepPlatform.GetStepByIdAsync(nextStepId);
        if (nextStep is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Trip? trip = await _tripPlatform.GetTripByIdAsync(step.TripId);
        if (trip is null)
            return NotFound(new Error(ETripErrorCodes.TripNotFoundById.ToStringValue(), "Aucun voyage lier a cette étape."));

        if (nextStep.TripId != step.TripId)
            return BadRequest(new Error(EStepErrorCodes.StepsNotInSameTrip.ToStringValue(), "Les étapes fournies ne sont pas dans le même voyage"));

        if (nextStep.Order < step.Order)
            return BadRequest(new Error(EStepErrorCodes.OrderNotInOrder.ToStringValue(), "L'étape suivante doit être après l'étape actuelle"));

        await _stepPlatform.MoveStepAfterAsync(trip, step, nextStep);

        return _mapper.Map<Step, StepDto>(step);
    }

    #endregion

    #region Update - One field
    /// <summary>
    /// Update step name
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    /// <param name="updateStepNameDto">updateStepNameDto</param>
    /// <returns>Step</returns>
    [HttpPatch]
    [Route("{stepId}/name")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> UpdateStepNameAsync([FromRoute] int stepId, [FromBody] UpdateStepNameDto updateStepNameDto)
    {
        ValidationResult validationResult = _updateStepNameValidator.Validate(updateStepNameDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Step? step = await _stepPlatform.GetByStepIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        await _stepPlatform.UpdateStepNameAsync(step, updateStepNameDto);

        return _mapper.Map<Step, StepDto>(step);
    }

    /// <summary>
    /// Update step description
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    /// <param name="updateStepDescriptionDto">updateStepDescriptionDto</param>
    [HttpPatch]
    [Route("{stepId}/description")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> UpdateStepDescriptionAsync([FromRoute] int stepId, [FromBody] UpdateStepDescriptionDto updateStepDescriptionDto)
    {
        ValidationResult validationResult = _updateStepDescriptionValidator.Validate(updateStepDescriptionDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Step? step = await _stepPlatform.GetByStepIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        await _stepPlatform.UpdateStepDescAsync(step, updateStepDescriptionDto);

        return _mapper.Map<Step, StepDto>(step);
    }

    /// <summary>
    /// Update step date
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    /// <param name="updateStepDateDto">updateStepDateDto</param>
    /// <returns>Step</returns>
    [HttpPatch]
    [Route("{stepId}/date")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> UpdateStepDateAsync([FromRoute] int stepId, [FromBody] UpdateStepDateDto updateStepDateDto)
    {
        ValidationResult validationResult = _updateStepDateValidator.Validate(updateStepDateDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Step? step = await _stepPlatform.GetByStepIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        await _stepPlatform.UpdateStepDateAsync(step, updateStepDateDto);

        return _mapper.Map<Step, StepDto>(step);
    }

    /// <summary>
    /// Update step location
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    /// <param name="updateStepLocationDto">updateStepLocationDto</param>
    /// <returns>Step</returns>
    [HttpPatch]
    [Route("{stepId}/location")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StepDto>> UpdateStepLocationAsync([FromRoute] int stepId, [FromBody] UpdateStepLocationDto updateStepLocationDto)
    {
        ValidationResult validationResult = _updateStepLocationValidator.Validate(updateStepLocationDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Step? step = await _stepPlatform.GetByStepIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        await _stepPlatform.UpdateStepLocationAsync(step, updateStepLocationDto);

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
    public async Task<ActionResult> DeleteStepByIdAsync([FromRoute] int stepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step is null)
            return NotFound(new Error(EStepErrorCodes.StepNotFoundById.ToStringValue(), "Etape non trouvé par id"));

        Trip? trip = await _tripPlatform.GetTripByIdAsync(step.TripId);

        await _stepPlatform.DeleteStepAsync(trip, step);
        return NoContent();
    }
    #endregion
}

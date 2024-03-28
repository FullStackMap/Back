using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AddTravel;
using Map.Platform.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Map.API.Controllers.Models.HttpError;

namespace Map.API.Controllers;

[Authorize]
[ApiController]
[ApiVersion(ApiControllerVersions.V1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class TravelController : ControllerBase
{
    #region Props

    private readonly IMapper _mapper;
    private readonly IValidator<AddTravelDto> _addTravelValidator;
    private readonly ITravelPlatform _travelPlatform;
    private readonly IStepPlatform _stepPlatform;

    #endregion

    #region Ctor

    public TravelController(IMapper mapper,
                            IValidator<AddTravelDto> addTravelValidator,
                            ITravelPlatform travelPlatform,
                            IStepPlatform stepPlatform)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _addTravelValidator = addTravelValidator ?? throw new ArgumentNullException(nameof(addTravelValidator));
        _travelPlatform = travelPlatform ?? throw new ArgumentNullException(nameof(travelPlatform));
        _stepPlatform = stepPlatform ?? throw new ArgumentNullException(nameof(stepPlatform));
    }

    #endregion

    /// <summary>
    /// Add Travel Between Steps
    /// </summary>
    /// <param name="addTravelDto">AddTravelDto</param>
    [HttpPost]
    [Route("AddTravelBetweenSteps")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<Error>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddTravelBetweenSteps([FromBody] AddTravelDto addTravelDto)
    {
        ValidationResult validationResult = await _addTravelValidator.ValidateAsync(addTravelDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Travel travel = _mapper.Map<AddTravelDto, Travel>(addTravelDto);
        Step? origin = await _stepPlatform.GetStepByIdAsync(travel.OriginStepId);
        if (origin == null)
            return BadRequest(new Error(nameof(EStepErrorCodes.StepNotFoundById), "L'étape d'origine n'a pas été trouvée"));

        Step? destination = await _stepPlatform.GetStepByIdAsync(travel.DestinationStepId);
        if (destination == null)
            return BadRequest(new Error(nameof(EStepErrorCodes.StepNotFoundById), "L'étape de destination n'a pas été trouvée"));

        if (origin.TripId != destination.TripId)
            return BadRequest(new Error(nameof(ETravelErrorCode.TravelBetweenSameTrip), "Le trajet doit être entre deux étapes du même voyage"));

        await _travelPlatform.AddTravelBetweenStepsAsync(origin, destination, travel);

        return Ok();
    }

    /// <summary>
    /// Remove Travel Before Step
    /// </summary>
    /// <param name="stepId">Step where to remove TravelBefore</param>
    [HttpDelete]
    [Route("RemoveTravelBeforeStep/{stepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemoveTravelBeforeStepAsync([FromRoute] int stepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step == null)
            return NotFound(new Error(nameof(EStepErrorCodes.StepNotFoundById), "L'étape n'a pas été trouvée"));

        await _travelPlatform.RemoveTravelBeforeStepAsync(step);

        return NoContent();
    }

    /// <summary>
    /// Remove Travel After Step
    /// </summary>
    /// <param name="stepId">Step where to remove TravelAfter</param>
    [HttpDelete]
    [Route("RemoveTravelAfterStep/{stepId}")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemoveTravelAfterStepAsync([FromRoute] int stepId)
    {
        Step? step = await _stepPlatform.GetStepByIdAsync(stepId);
        if (step == null)
            return NotFound(new Error(nameof(EStepErrorCodes.StepNotFoundById), "L'étape n'a pas été trouvée"));

        await _travelPlatform.RemoveTravelAfterStepAsync(step);

        return NoContent();
    }

}
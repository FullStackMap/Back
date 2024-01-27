using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Map.API.Models.TripDto;
using Map.Domain.Entities;
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

    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public TripController(ITripPlatform tripPlatform, UserManager<MapUser> userManager, IValidator<AddTripDto> addTripValidator, IMapper mapper)
    {
        _tripPlatform = tripPlatform ?? throw new ArgumentNullException(nameof(tripPlatform));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _addTripValidator = addTripValidator ?? throw new ArgumentNullException(nameof(addTripValidator));
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

    [HttpPost]
    [Route("")]
    [MapToApiVersion(ApiControllerVersions.V1)]
    public async Task<IActionResult> AddTrip([FromBody] AddTripDto addTripDto)
    {
        ValidationResult validationResult = await _addTripValidator.ValidateAsync(addTripDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));

        Trip entity = _mapper.Map<AddTripDto, Trip>(addTripDto);
        await _tripPlatform.AddTripAsync(entity);

        //TODO : Add Created return when GetTripById will be implemented
        return Ok(_mapper.Map<Trip, TripDto>(entity));
    }
}

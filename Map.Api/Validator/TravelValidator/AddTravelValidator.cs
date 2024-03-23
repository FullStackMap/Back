using FluentValidation;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AddTravel;
using Map.Platform.Interfaces;

namespace Map.API.Validator.TravelValidator;

internal class AddTravelValidator : AbstractValidator<AddTravelDto>
{
    public AddTravelValidator(IStepPlatform stepPlatform)
    {
        if (stepPlatform is null)
            throw new ArgumentNullException(nameof(stepPlatform));


        #region Dto
        RuleFor(dto => dto)
            //Check if the Travel is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.TravelNotEmpty))
            .WithMessage("Les données du trajet sont requises");
        #endregion

        #region TransportMode
        RuleFor(dto => dto.TransportMode)
            //Check if the TransportMode is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.TransportModeNotEmpty))
            .WithMessage("Le mode de transport est requis")
            .Unless(dto => dto is null);
        #endregion

        #region Distance
        RuleFor(dto => dto.Distance)
            //Check if the Distance is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.DistanceNotEmpty))
            .WithMessage("La distance pour ce trajet est requise")
            .Unless(dto => dto is null);
        #endregion

        #region Duration
        RuleFor(dto => dto.Duration)
            //Check if the DestinationStepId is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.DurationNotEmpty))
            .WithMessage("Le temp hestimer de ce trajet est requis")
            .Unless(dto => dto is null);
        #endregion

        #region OriginStepId
        RuleFor(dto => dto.OriginStepId)
            //Check if the DestinationStepId is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.OriginStepIdNotEmpty))
            .WithMessage("L'étape de départ est requise")
            .Unless(dto => dto is null);
        #endregion

        #region DestinationStepId
        RuleFor(dto => dto.DestinationStepId)
            //Check if the DestinationStepId is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.DestinationStepIdNotEmpty))
            .WithMessage("L'étape de destination est requise")
            .Unless(dto => dto is null)

            //Check if OriginStepId and DestinationStepId are not the same
            .NotEqual(dto => dto.OriginStepId)
            .WithErrorCode(nameof(ETravelErrorCode.OriginStepIdAndDestinationStepIdNotTheSame))
            .WithMessage("L'étape de départ et l'étape de destination ne peuvent pas être les mêmes")

            //Check if OriginStepId and DestinationStepId are in same Trip
            .MustAsync(async (dto, destinationStepId, cancellationToken) =>
            {
                Step? originStep = await stepPlatform.GetByStepIdAsync(dto.OriginStepId);
                Step? destinationStep = await stepPlatform.GetByStepIdAsync(dto.DestinationStepId);

                if (originStep is null || destinationStep is null)
                    return false;

                return originStep.TripId == destinationStep.TripId;
            })
            .WithErrorCode(nameof(ETravelErrorCode.OriginStepIdAndDestinationStepIdNotInSameTrip))
            .WithMessage("L'étape de départ et l'étape de destination doivent être dans le même voyage")

            //Check if OriginStep.Order is DestinationStep.Order - 1
            .MustAsync(async (dto, destinationStepId, cancellationToken) =>
            {
                Step? originStep = await stepPlatform.GetByStepIdAsync(dto.OriginStepId);
                Step? destinationStep = await stepPlatform.GetByStepIdAsync(dto.DestinationStepId);

                if (originStep is null || destinationStep is null)
                    return false;

                return originStep.Order == destinationStep.Order - 1;
            })
            .WithErrorCode(nameof(ETravelErrorCode.OriginStepOrderAndDestinationStepOrderNotSequential))
            .WithMessage("L'étape de départ doit être l'étape précédente de la destination");
        #endregion

        #region TravelRoad
        RuleFor(dto => dto.TravelRoad)
            //Check if the DestinationStepId is not empty
            .NotEmpty()
            .WithErrorCode(nameof(ETravelErrorCode.TravelRoadNotEmpty))
            .WithMessage("Les données de la route de ce trajet sont requise")
            .Unless(dto => dto is null);
        #endregion TravelRoad

    }
}

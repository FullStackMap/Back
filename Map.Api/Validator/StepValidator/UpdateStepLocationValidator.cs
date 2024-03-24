using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Step;

namespace Map.API.Validator.StepValidator;

internal class UpdateStepLocationValidator : AbstractValidator<UpdateStepLocationDto>
{
    public UpdateStepLocationValidator()
    {
        RuleFor(dto => dto)
            //check if the dto is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Le Dto est requis");

        RuleFor(dto => dto.Latitude)
            //check if the latitude is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.LatitudeNotEmpty.ToStringValue())
            .WithMessage("La latitude est obligatoire");

        RuleFor(dto => dto.Longitude)
            //check if the longitude is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.LongitudeNotEmpty.ToStringValue())
            .WithMessage("La longitude est obligatoire");
    }
}

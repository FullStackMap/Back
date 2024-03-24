using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AddTravel;
using Map.Domain.Models.Step;

namespace Map.API.Validator.StepValidator;

internal class AddStepValidator : AbstractValidator<AddStepDto>
{
    public AddStepValidator(IValidator<AddTravelDto> addTravelValidator)
    {
        if (addTravelValidator is null)
            throw new ArgumentNullException(nameof(addTravelValidator));

        RuleFor(dto => dto)
            //check if the dto is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Le Dto est requis");

        RuleFor(dto => dto.Name)
            //check if the name is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.NameNotEmpty.ToStringValue())
            .WithMessage("Le nom est obligatoire")
            //Check if the name is more than 3 characters
            .MinimumLength(3)
            .WithErrorCode(EStepErrorCodes.NameMinLength.ToStringValue())
            .WithMessage("Le nom doit contenir au moins 3 caractères")
            //Check if the name is less than 50 characters
            .MaximumLength(50)
            .WithErrorCode(EStepErrorCodes.NameMaxLength.ToStringValue())
            .WithMessage("Le nom ne doit pas dépasser 50 caractères");

        RuleFor(dto => dto.Description)
            //check if the description len is less than 500
            .MaximumLength(500)
            .WithErrorCode(EStepErrorCodes.DescriptionMaxLength.ToStringValue())
            .WithMessage("La description ne doit pas dépasser 500 caractères");

        RuleFor(dto => dto.Latitude)
            //check if the latitude is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.LatitudeNotEmpty.ToStringValue())
            .WithMessage("Latitude is required");

        RuleFor(dto => dto.Longitude)
            //check if the longitude is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.LongitudeNotEmpty.ToStringValue())
            .WithMessage("Longitude is required");
    }
}

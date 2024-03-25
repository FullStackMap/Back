using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Step;

namespace Map.API.Validator.StepValidator;

internal class UpdateStepNameValidator : AbstractValidator<UpdateStepNameDto>
{
    public UpdateStepNameValidator()
    {
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
    }
}

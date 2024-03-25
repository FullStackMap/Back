using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Step;

namespace Map.API.Validator.StepValidator;

internal class UpdateStepDescriptionValidator : AbstractValidator<UpdateStepDescriptionDto>
{
    public UpdateStepDescriptionValidator()
    {
        RuleFor(dto => dto)
            //check if the dto is not empty
            .NotEmpty()
            .WithErrorCode(EStepErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Le Dto est requis");

        RuleFor(dto => dto.Description)
            //check if the description len is less than 500
            .MaximumLength(500)
            .WithErrorCode(EStepErrorCodes.DescriptionMaxLength.ToStringValue())
            .WithMessage("La description ne doit pas dépasser 500 caractères");
    }
}

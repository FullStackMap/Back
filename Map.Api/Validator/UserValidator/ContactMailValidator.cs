using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.EmailDto;

namespace Map.API.Validator.UserValidator;

public class ContactMailValidator : AbstractValidator<MailDto>
{
    public ContactMailValidator()
    {

        RuleFor(dto => dto)
            .NotEmpty()
            .WithErrorCode(EUserErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Le dto est requis");

        #region Email
        RuleFor(dto => dto.Email)
            //Check if the email is not empty
            .NotEmpty()
            .WithErrorCode(EUserErrorCodes.EmailNotEmpty.ToStringValue())
            .WithMessage("L'email est requis")

            //Check that the password is in this format
            .EmailAddress()
            .WithErrorCode(EUserErrorCodes.EmailNotValid.ToStringValue())
            .WithMessage("Le champ Email doit être un Email");
        #endregion

        #region Subject

        RuleFor(x => x.Subject)
            //Check if the subject is not empty
            .NotEmpty()
            .WithErrorCode(EUserErrorCodes.EmailSubjectNotEmpty.ToStringValue())
            .WithMessage("Subject is required");
        #endregion

        #region Body
        RuleFor(x => x.Body)
            //check if the body is not empty
            .NotEmpty()
            .WithErrorCode(EUserErrorCodes.EmailBodyNotEmpty.ToStringValue())
            .WithMessage("Message is required");

        #endregion

        #region Name
        RuleFor(x => x.Name)
            //check if the name is not empty
            .NotEmpty()
            .WithErrorCode(EUserErrorCodes.EmailNameNotEmpty.ToStringValue())
            .WithMessage("Name is required");

        #endregion
    }
}

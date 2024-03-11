using FluentValidation;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.UserValidator;

public class UpdateUserMailValidator : AbstractValidator<UpdateUserMailDto>
{
    public UpdateUserMailValidator(UserManager<MapUser> userManager)
    {
        if (userManager is null)
            throw new ArgumentNullException(nameof(userManager));

        RuleFor(dto => dto)
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("The dto is required");

        #region Email
        RuleFor(dto => dto.Mail)
            //Check if the email is not empty
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.EmailNotEmpty.ToStringValue())
            .WithMessage("Email is required")
            //Check that the password is in this format
            .EmailAddress()
            .WithErrorCode(EMapUserErrorCodes.EmailNotValid.ToStringValue())
            .WithMessage("Email field must be an Email")
            //Check if the email is used by any user
            .MustAsync(async (dto, email, cancellationToken) =>
            {
                MapUser? user = await userManager.FindByEmailAsync(email.ToString());
                return user is not null;
            })
            .WithErrorCode(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue())
            .WithMessage("No account found with this mail");
        #endregion
    }
}

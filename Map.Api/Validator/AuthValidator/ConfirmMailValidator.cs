using FluentValidation;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.AuthValidator;

public class ConfirmMailValidator : AbstractValidator<ConfirmMailDto>
{
    #region Props

    private readonly UserManager<MapUser> _userManager;

    #endregion

    public ConfirmMailValidator(UserManager<MapUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        RuleFor(dto => dto)
            //Dto is required
            .NotEmpty()
            .WithErrorCode(EAuthErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Dto is required");

        #region Email
        RuleFor(dto => dto.Email)
            //Check if mail is not empty
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.EmailNotEmpty.ToStringValue())
            .WithMessage("Email is required")
            //Check if mail is typeof MAIL
            .EmailAddress()
            .WithErrorCode(EMapUserErrorCodes.EmailNotValid.ToStringValue())
            .WithMessage("The mail muste be an valid email")
            //check if the user exist by mail
            .MustAsync(async (dto, email, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByEmailAsync(email.ToString());
                return user is not null;
            })
            .WithErrorCode(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue())
            .WithMessage("User not found by mail"); ;
        #endregion

        #region Token
        RuleFor(dto => dto.Token)
            .NotEmpty()
            .WithErrorCode(EAuthErrorCodes.TokenNotEmpty.ToStringValue())
            .WithMessage("Token is required");
        #endregion
    }
}

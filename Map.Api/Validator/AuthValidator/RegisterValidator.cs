using FluentValidation;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.AuthValidator;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    #region Props

    private readonly UserManager<MapUser> _userManager;

    #endregion

    public RegisterValidator(UserManager<MapUser> userManager)
    {
        RuleFor(dto => dto)
            .NotEmpty()
            .WithErrorCode(nameof(EAuthErrorCodes.DtoNotNull))
            .WithMessage("The dto is required");

        #region Username
        RuleFor(dto => dto.Username)
            //check if the username is not empty
            .NotEmpty()
            .WithErrorCode(nameof(EMapUserErrorCodes.UserNameNotEmpty))
            .WithMessage("Username is required")
            //check if the username is unique
            .MustAsync(async (dto, username, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByNameAsync(username.ToString());
                return user is null;
            })
            .WithErrorCode(nameof(EMapUserErrorCodes.UsernameNotUnique))
            .WithMessage("Username muste be unique");

        #endregion

        #region Email
        RuleFor(dto => dto.Email)
            //Check if the email is not empty
            .NotEmpty()
            .WithErrorCode(nameof(EMapUserErrorCodes.EmailNotEmpty))
            .WithMessage("Email is required")
            //Check that the password is in this format xx@xx.xx
            .EmailAddress()
            .WithErrorCode(nameof(EMapUserErrorCodes.EmailNotValid))
            .WithMessage("Email field must be an Email")
            //check if the email is unique
            .MustAsync(async (dto, email, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByEmailAsync(email.ToString());
                return user is null;
            })
            .WithErrorCode(nameof(EMapUserErrorCodes.EmailNotUnique))
            .WithMessage("Email muste be unique");
        #endregion

        #region Password
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithErrorCode(nameof(EMapUserErrorCodes.PasswordNotEmpty))
            .WithMessage("Password is required");

        #endregion

        #region ConfirmPassword
        RuleFor(dto => dto.ConfirmPassword)
            //Check is the ConfirmPassword is not required
            .NotEmpty()
           .WithErrorCode(nameof(EMapUserErrorCodes.ConfirmPasswordNotEmpty))
           .WithMessage("ConfirmPassword is required")
            //Check if ConfirmPassword is Equal thant password
            .Equal(dto => dto.Password)
            .Unless(dto => dto.Password is null
            && dto.ConfirmPassword is null
            && !string.IsNullOrWhiteSpace(dto.Password)
            && !string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            .WithErrorCode(nameof(EMapUserErrorCodes.ConfirmPasswordMustEqualPassword))
            .WithMessage("Confirm password field must be equal than password field");
        _userManager = userManager;

        #endregion

    }
}
using FluentValidation;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.AuthValidator;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    private readonly UserManager<MapUser> _userManager;
    public ResetPasswordValidator(UserManager<MapUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(EAuthErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Dto is required");

        #region Email
        RuleFor(dto => dto.Email)
            .NotNull()
            .WithErrorCode(EMapUserErrorCodes.EmailNotEmpty.ToStringValue())
            .WithMessage("Email is required")
            //Check if mail is valid
            .EmailAddress()
            .WithErrorCode(EMapUserErrorCodes.EmailNotValid.ToStringValue())
            .WithMessage("Email is not valid")
            //check if the user exist by mail
            .MustAsync(async (dto, email, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByEmailAsync(email.ToString());
                return user is not null;
            })
            .WithErrorCode(EMapUserErrorCodes.UserNotFoundByEmail.ToStringValue())
            .WithMessage("User not found by mail");
        #endregion

        #region Password
        RuleFor(dto => dto.Password)
            //check if password is not null
            .NotNull()
            .WithErrorCode(EMapUserErrorCodes.PasswordNotEmpty.ToStringValue())
            .WithMessage("Password is required");

        #endregion

        #region ConfirmPassword
        RuleFor(dto => dto.PasswordConfirmation)
            //checj if confirm password is not empty
            .NotNull()
            .WithErrorCode(nameof(EMapUserErrorCodes))
            .WithMessage("Password Confirmation is required")
            //Check if confirm password and password is equals
            .Equal(dto => dto.Password)
            .Unless(dto => dto.Password is null
                           && dto.PasswordConfirmation is null
                           && !string.IsNullOrWhiteSpace(dto.Password)
                           && !string.IsNullOrWhiteSpace(dto.PasswordConfirmation))
            .WithErrorCode(EMapUserErrorCodes.ConfirmPasswordMustEqualPassword.ToStringValue())
            .WithMessage("Confirm password must be equal than password");
        #endregion

        #region Token
        RuleFor(dto => dto.Token)
            //confirm is token is not null
            .NotNull()
            .NotEmpty()
            .Unless(dto => dto is null)
            .WithErrorCode(EAuthErrorCodes.TokenNotEmpty.ToStringValue())
            .WithMessage("Token is required");

        #endregion
    }
}
using FluentValidation;
using Map.API.Extension;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.AuthValidator;

internal class RegisterValidator : AbstractValidator<RegisterDto>
{
    #region Props

    private readonly UserManager<MapUser> _userManager;

    #endregion

    public RegisterValidator(UserManager<MapUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        RuleFor(dto => dto)
            .NotEmpty()
            .WithErrorCode(EAuthErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Le DTO est requis");

        #region Username
        RuleFor(dto => dto.Username)
            //check if the username is not empty
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.UserNameNotEmpty.ToStringValue())
            .WithMessage("Le nom d'utilisateur est requis")
            //check if the username is unique
            .MustAsync(async (dto, username, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByNameAsync(username.ToString());
                return user is null;
            })
            .WithErrorCode(EMapUserErrorCodes.UsernameNotUnique.ToStringValue())
            .WithMessage("Ce nom d'utilisateur est déjà utiliser");

        #endregion

        #region Email
        RuleFor(dto => dto.Email)
            //Check if the email is not empty
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.EmailNotEmpty.ToStringValue())
            .WithMessage("L'email est requis")
            //Check that the password is in this format xx@xx.xx
            .EmailAddress()
            .WithErrorCode(EMapUserErrorCodes.EmailNotValid.ToStringValue())
            .WithMessage("L'email n'est pas valide")
            //check if the email is unique
            .MustAsync(async (dto, email, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByEmailAsync(email.ToString());
                return user is null;
            })
            .WithErrorCode(EMapUserErrorCodes.EmailNotUnique.ToStringValue())
            .WithMessage("Cet email est déjà assosier à un autre compte");
        #endregion

        #region Password
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithErrorCode(EMapUserErrorCodes.PasswordNotEmpty.ToStringValue())
            .WithMessage("Le mot de passe est requis");

        #endregion

        #region ConfirmPassword
        RuleFor(dto => dto.ConfirmPassword)
            //Check is the ConfirmPassword is not required
            .NotEmpty()
           .WithErrorCode(EMapUserErrorCodes.ConfirmPasswordNotEmpty.ToStringValue())
           .WithMessage("La confirmation de mot de passe est requise")
            //Check if ConfirmPassword is Equal thant password
            .Equal(dto => dto.Password)
            .Unless(dto => dto.Password is null
            && dto.ConfirmPassword is null
            && !string.IsNullOrWhiteSpace(dto.Password)
            && !string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            .WithErrorCode(EMapUserErrorCodes.ConfirmPasswordMustEqualPassword.ToStringValue())
            .WithMessage("Le champ \"Confirmation du mot de passe\" doit être égal au champ \"Mot de passe\".");

        #endregion

    }
}
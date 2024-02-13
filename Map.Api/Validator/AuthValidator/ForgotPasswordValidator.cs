using FluentValidation;
using Map.Domain.Entities;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace Map.API.Validator.AuthValidator;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    private readonly UserManager<MapUser> _userManager;
    public ForgotPasswordValidator(UserManager<MapUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        RuleFor(dto => dto)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(nameof(EAuthErrorCodes.DtoNotNull))
            .WithMessage("Dto is required");

        #region Email
        RuleFor(dto => dto.Email)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(nameof(EMapUserErrorCodes.EmailNotEmpty))
            .WithMessage("Email is required")
            //Check if email is valid than xx@xx.xx
            .EmailAddress()
            .WithErrorCode(nameof(EMapUserErrorCodes.EmailNotValid))
            .WithMessage("Email not valid")
            //check if the user exist by mail
            .MustAsync(async (dto, email, cancellationToken) =>
            {
                MapUser? user = await _userManager.FindByEmailAsync(email.ToString());
                return user is not null;
            })
            .WithErrorCode(nameof(EMapUserErrorCodes.UserNotFoundByEmail))
            .WithMessage("User not found by mail");
        
        #endregion
    }
}